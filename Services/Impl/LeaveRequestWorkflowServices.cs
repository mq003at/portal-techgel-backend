namespace portal.Services;

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using portal.Db;
using portal.Documents.Props;
using portal.DTOs;
using portal.Enums;
using portal.Helpers;
using portal.Models;
using Serilog;

public class LeaveRequestWorkflowService : BaseService<
    LeaveRequestWorkflow,
    LeaveRequestWorkflowDTO,
    CreateLeaveRequestWorkflowDTO,
    UpdateLeaveRequestWorkflowDTO>,
    ILeaveRequestWorkflowService
{
    private new readonly IFileStorageService _storage;
    private new readonly DocumentOptions _docOpts;

    public LeaveRequestWorkflowService(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<LeaveRequestWorkflowService> logger,
        IOptions<DocumentOptions> docOpts,
        IFileStorageService storage
    ) : base(context, mapper, logger)
    {
        _docOpts = docOpts.Value;
        _storage = storage;
    }

    public async Task<List<LeaveRequestNodeDTO>> GenerateStepsAsync(
        CreateLeaveRequestWorkflowDTO dto,
        LeaveRequestWorkflow workflow)
    {
        // Get the last node ID (handle empty table)
        var lastId = await _context.LeaveRequestNodes.AnyAsync()
            ? await _context.LeaveRequestNodes.MaxAsync(x => x.Id)
            : 0;

        // Calculate number of leave days (min 0.5)
        var totalDays = DateHelper.CalculateLeaveDays(dto.StartDate, (int)dto.StartDateDayNightType, dto.EndDate, (int)dto.EndDateDayNightType);

        if (totalDays < 0.4)
            throw new ArgumentException("End date must be after start date and at least half a day apart.");

        // Fetch annual leave and update DTO
        var employee = await _context.Employees
            .Where(e => e.Id == dto.EmployeeId)
            .FirstOrDefaultAsync();

        if (employee == null)
            throw new InvalidOperationException($"Employee {dto.EmployeeId} not found.");


        workflow.TotalDays = (float)totalDays;
        workflow.EmployeeAnnualLeaveTotalDays = employee.CompanyInfo.AnnualLeaveTotalDays;
        workflow.FinalEmployeeAnnualLeaveTotalDays = employee.CompanyInfo.AnnualLeaveTotalDays - (float)totalDays;
        // Get Director (Executive) and HR Head
        var directorId = await _context.OrganizationEntities
            .Where(e => e.Id == 3)
            .Select(e => e.ManagerId)
            .FirstOrDefaultAsync();

        if (employee.RoleInfo.SupervisorId is null || directorId is null)
            throw new InvalidOperationException("Workflow is missing required manager IDs.");

        // Build up receiver IDs for workflow
        workflow.ReceiverIds.AddRange(new[] { dto.SenderId, employee.RoleInfo.SupervisorId.Value, directorId.Value });

        // Compose workflow steps
        var steps = new List<(string Name, LeaveApprovalStepType StepType, int Status, List<int> Approvers, List<int> Approved)>
    {
        ("Tạo yêu cầu nghỉ phép", LeaveApprovalStepType.CreateForm, 1, new() { dto.SenderId }, new() { dto.SenderId }),
        ("Quản lý trực thuộc ký", LeaveApprovalStepType.ManagerApproval, 0, new() { employee.RoleInfo.SupervisorId.Value }, new())
    };

        if (totalDays >= 3.0)
        {
            var hrHeadId = await _context.OrganizationEntities
                .Where(e => e.Id == 11)
                .Select(e => e.ManagerId)
                .FirstOrDefaultAsync();

            if (hrHeadId is null)
                throw new InvalidOperationException("HR Head does not have a valid manager.");

            steps.Add(("Trưởng phòng nhân sự ký", LeaveApprovalStepType.HRHeadApproval, 2, new() { hrHeadId.Value }, new()));
            workflow.ReceiverIds.Add(hrHeadId.Value);
        }

        steps.Add(("Ký xác nhận cuối cùng", LeaveApprovalStepType.ExecutiveApproval, 2, new() { directorId.Value }, new()));

        // Build and add nodes
        var nodes = new List<LeaveRequestNode>();
        for (int i = 0; i < steps.Count; i++)
        {
            var step = steps[i];
            var newId = lastId + i + 1;

            var node = new LeaveRequestNode
            {
                LeaveRequestWorkflowId = workflow.Id,
                StepType = step.StepType,
                MainId = $"AS-S{i}-{DateTime.Now:dd.MM.yyyy}/{newId}",
                Name = step.Name,
                SenderId = dto.SenderId,
                Status = (GeneralWorkflowStatusType)step.Status,
                ApprovedByIds = step.Approvers,
                HasBeenApprovedByIds = step.Approved,
                ApprovedDates = new(),
                DocumentIds = new() { 14 }
            };

            nodes.Add(node);
            _context.LeaveRequestNodes.Add(node);
        }
        _context.LeaveRequestWorkflows.Update(workflow);
        await _context.SaveChangesAsync();
        return _mapper.Map<List<LeaveRequestNodeDTO>>(nodes);
    }

    public async Task<bool> ApproveNodeAsync(int nodeId, int approverId, string? comment = null)
    {
        var node = await _context.LeaveRequestNodes.FindAsync(nodeId);
        if (node == null) return false;

        // Ensure array never null (đề phòng DB đang null)
        node.ApprovedByIds ??= new List<int>();
        node.HasBeenApprovedByIds ??= new List<int>();

        // LOG
        _logger.LogInformation(
            "Approving node {NodeId} by approver {ApproverId} with comment: {Comment}",
            nodeId, approverId, comment
        );

        // Block nếu approverId không có quyền approve
        if (!node.ApprovedByIds.Contains(approverId))
            throw new InvalidOperationException(
                $"Approver {approverId} is not allowed to approve node {nodeId}."
            );

        // Block nếu đã approve rồi
        if (node.HasBeenApprovedByIds.Contains(approverId))
            throw new InvalidOperationException(
                $"Node {nodeId} has already been approved by {approverId}."
            );

        // Thêm vào danh sách đã approve
        node.HasBeenApprovedByIds.Add(approverId);

        node.ApprovedDates ??= new List<DateTime>();
        node.ApprovedDates.Add(DateTime.UtcNow);

        // Check đã đủ approved hay chưa
        var hasNodeAllApproved = Helpers.ArrayHelper.AreArraysEqual(
            node.ApprovedByIds, node.HasBeenApprovedByIds
        );

        if (hasNodeAllApproved)
        {
            node.Status = GeneralWorkflowStatusType.Approved;
            _logger.LogInformation(
                "Node {NodeId} has been fully approved by all required approvers.",
                nodeId
            );
        }

        _context.LeaveRequestNodes.Update(node);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<LeaveRequestNodeDTO>> GetNodesByWorkflowIdAsync(int workflowId)
    {
        var nodes = await _context.LeaveRequestNodes
            .Where(n => n.LeaveRequestWorkflowId == workflowId)
            .ToListAsync();

        return _mapper.Map<IEnumerable<LeaveRequestNodeDTO>>(nodes);
    }

    public async Task<bool> FinalizeIfCompleteAsync(int workflowId)
    {
        var workflow = await _context.LeaveRequestWorkflows
            .Include(w => w.LeaveRequestNodes)
            .FirstOrDefaultAsync(w => w.Id == workflowId);

        if (workflow == null) return false;

        var allApproved = workflow.LeaveRequestNodes.All(n =>
            n.Status == GeneralWorkflowStatusType.Approved);

        if (allApproved)
        {
            workflow.Status = GeneralWorkflowStatusType.Approved;
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public override async Task<LeaveRequestWorkflowDTO> CreateAsync(
        CreateLeaveRequestWorkflowDTO dto
    )
    {

        var entity = _mapper.Map<LeaveRequestWorkflow>(dto);
        var employeeMainId = await _context.Employees.Where(e => e.Id == dto.EmployeeId).Select(e => e.MainId).FirstOrDefaultAsync();

        // Tự điền thông tin
        entity.Name = "AL-" + employeeMainId + "/" + entity.StartDate.ToString("dd.MM") + entity.EndDate.ToString("dd.MM");
        entity.Name = "Hố sơ nghỉ phép nhân viên" + employeeMainId + "Từ: " + entity.StartDate.ToString("HH:mm DD/dd/MM/yyyy") + " tới ngày " + entity.EndDate.ToString("HH:mm DD/dd/MM/yyyy");

        _dbSet.Add(entity);
        await _context.SaveChangesAsync(); // Để có entity.Id

        _logger.LogInformation(
            "Created new LeaveRequestWorkflow with ID {Id} for Employee {EmployeeId}",
            entity.Id,
            entity.EmployeeId
        );

        // ✅ Sinh node
        var nodes = await GenerateStepsAsync(dto, entity);
        var finalDto = _mapper.Map<LeaveRequestWorkflowDTO>(entity);
        _logger.LogError(
            "Generated {NodeCount} employeeAnualLeave for this {WorkflowId} in dto",
            entity.EmployeeAnnualLeaveTotalDays,
            finalDto.EmployeeAnnualLeaveTotalDays
        );
        // Populate thêm metadata nếu cần
        await PopulateMetadataAsync(finalDto);
        finalDto.LeaveRequestNodes = nodes;

        return finalDto;
    }

    public override async Task<LeaveRequestWorkflowDTO?> UpdateAsync(
        int id,
        UpdateLeaveRequestWorkflowDTO dto
    )
    {
        var workflow = await base.UpdateAsync(id, dto);

        if (workflow == null)
            return null;
        // Populate metadata after update
        await PopulateMetadataAsync(workflow);

        return workflow;
    }

    public override async Task<LeaveRequestWorkflowDTO?> GetByIdAsync(int id)
    {
        var workflow = await base.GetByIdAsync(id);
        if (workflow is null)
        {
            throw new KeyNotFoundException($"LeaveRequestWorkflow with ID {id} not found.");
        }
        await PopulateMetadataAsync(workflow);
        foreach (var node in workflow.LeaveRequestNodes)
        {
            await PopulateMetadataAsync(node, workflow.SenderName);
        }
        return workflow;
    }

    public override async Task<IEnumerable<LeaveRequestWorkflowDTO>> GetAllAsync()
    {
        var workflows = await base.GetAllAsync();
        var workflowList = workflows.ToList();

        // Populate metadata for each workflow
        foreach (var workflow in workflowList)
        {
            await PopulateMetadataAsync(workflow);
            // foreach (var node in workflow.LeaveRequestNodes)
            // {
            //     await PopulateMetadataAsync(node);
            // }
        }


        return workflowList;
    }

    public async Task PopulateMetadataAsync(LeaveRequestWorkflowDTO workflow)
    {
        // Populate EmployeeName
        _logger.LogInformation(
            "Populating metadata for LeaveRequestWorkflow with ID {Id} which has empid {empid} and assignedid {ass}",
            workflow.Id, workflow.EmployeeId, workflow.WorkAssignedToId
        );
        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == workflow.EmployeeId);
        var assignee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == workflow.WorkAssignedToId);
        var hr = await _context.Employees.FirstOrDefaultAsync(e => e.Id == 4);
        var generalDirector = await _context.Employees.FirstOrDefaultAsync(e => e.Id == 1);

        if (employee == null || assignee == null || hr == null || generalDirector == null)
        {
            throw new InvalidOperationException("One or more required employees (employee, assignee, hr, generalDirector) could not be found.");
        }
        else
        {
            // Get supervisor from employee.RoleInfo.SupervisorId
            var supervisor = await _context.Employees.FirstOrDefaultAsync(e => e.Id == employee.RoleInfo.SupervisorId);
            var supervisorName = supervisor != null
            ? supervisor.FirstName + " " + supervisor.MiddleName + " " + supervisor.LastName
            : "";
            var supervisorPosition = supervisor?.CompanyInfo.Position ?? "";

            if (employee != null && assignee != null)
            {
                workflow.SenderId = employee.Id;
                workflow.EmployeeMainId = employee.MainId ?? "";
                workflow.SenderName = employee.FirstName + " " + employee.MiddleName + " " + employee.LastName;
                workflow.EmployeeName = employee.FirstName + " " + employee.MiddleName + " " + employee.LastName;
                workflow.WorkAssignedToName = assignee.FirstName + " " + assignee.MiddleName + " " + assignee.LastName;
                workflow.WorkAssignedToPosition = assignee.CompanyInfo.Position ?? "";
                workflow.WorkAssignedToPhone = assignee.CompanyInfo.CompanyPhoneNumber ?? "";
                workflow.WorkAssignedToEmail = assignee.CompanyInfo.CompanyEmail ?? "";
                workflow.WorkAssignedToHomeAdress = assignee.PersonalInfo.Address ?? "";
            }
            workflow.ReceiverNames = await GetNamesByIdsAsync(workflow.ReceiverIds.ToList());
            workflow.HasBeenApprovedByNames = await GetNamesByIdsAsync(workflow.HasBeenApprovedByIds.ToList());
            workflow.LeaveRequestNodes = _context.LeaveRequestNodes
                .Where(n => n.LeaveRequestWorkflowId == workflow.Id)
                .Select(n => _mapper.Map<LeaveRequestNodeDTO>(n))
                .ToList();

            foreach (var node in workflow.LeaveRequestNodes)
            {
                await PopulateMetadataAsync(node, workflow.SenderName);
            }

            var docxStream = new MemoryStream();
            if (workflow.Status == GeneralWorkflowStatusType.Approved)
            {
                // If the workflow is approved, generate the document
                docxStream = GenerateLeaveRequestDocument(
                employee,
                assignee,
                hr,
                generalDirector,
                supervisor,
                workflow,
                true
                );
            }
            else
            {
                docxStream = GenerateLeaveRequestDocument(
                employee,
                assignee,
                hr,
                generalDirector,
                supervisor,
                workflow,
                false
                );
            }

            docxStream.Position = 0; // Reset stream for reading
            var fileName = (workflow.Status == GeneralWorkflowStatusType.Approved ? "SIGNED" : "INFO") + " -LeaveRequestForm.docx";
            var attachment = new Attachment
            {
                FileName = fileName,
                ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                FileContent = docxStream.ToArray()
            };

            if (workflow.Attachments == null)
            {
                workflow.Attachments = new List<Attachment> { attachment };
            }
            else
            {
                workflow.Attachments.Add(attachment);
            }
        }


    }

    private async Task PopulateMetadataAsync(LeaveRequestNodeDTO node, string SenderName)
    {
        // Populate EmployeeName
        // Populate names from IDs
        node.HasBeenApprovedByNames = await GetNamesByIdsAsync(node.HasBeenApprovedByIds.ToList());
        node.ApprovedByNames = await GetNamesByIdsAsync(node.ApprovedByIds.ToList());
        node.SenderName = SenderName;
    }

    private async Task<List<string>> GetNamesByIdsAsync(List<int> ids)
    {
        return await _context.Employees
            .Where(e => ids.Contains(e.Id))
            .Select(e => e.FirstName + " " + e.MiddleName + " " + e.LastName)
            .ToListAsync();
    }

    private MemoryStream GenerateLeaveRequestDocument(Employee employee, Employee assignee, Employee hr, Employee generalDirector, Employee supervisor, LeaveRequestWorkflowDTO leaveRequest, bool isSigned = false)
    {
        string templatePath = Path.Combine("Helpers", "Documents", "Template", "TEMPLATE-LeaveRequestTemplate.docx");

        DateTime? employeeSignDate = leaveRequest.ApprovedDates?.FirstOrDefault();
        DateTime? supervisorSignDate = leaveRequest.ApprovedDates?.ElementAtOrDefault(1);
        DateTime? hrSignDate = leaveRequest.ApprovedDates?.ElementAtOrDefault(2);
        DateTime? generalDirectorSignDate = leaveRequest.ApprovedDates?.ElementAtOrDefault(3);


        var props = new LeaveRequestProps(
    templatePath,

    // --- Top Section ---
    employee.FirstName + " " + employee.MiddleName + " " + employee.LastName, // EmployeeName
    (DateTime)leaveRequest.StartDate,                                           // LeaveRequestStartHour
    employee.CompanyInfo.Department ?? "",                                     // Department
    (DateTime)leaveRequest.EndDate,                                             // LeaveRequestEndHour
    employee.CompanyInfo.Position ?? "",                                       // Position
    leaveRequest.Reason,                                                                    // Reason
    leaveRequest.LeaveAprrovalCategory.ToString(),                                          // LeaveApprovalCategory

    // --- Assignee Section ---
    assignee.FirstName + " " + assignee.MiddleName + " " + assignee.LastName + (employee.Id == assignee.Id ? " (Sẵn sàng nhận điện thoại / Hỗ trợ từ xa)" : ""),  // AssigneeName
    assignee.PersonalInfo.PersonalPhoneNumber ?? "",                           // AssigneePhoneNumber
    assignee.CompanyInfo.CompanyEmail ?? "",                                   // AssigneeEmail (you might want to confirm which field)
    assignee.PersonalInfo.Address ?? "",                                       // AssigneeAddress

    // --- Leave Stats ---
    leaveRequest.EmployeeAnnualLeaveTotalDays,                                              // EmployeeAnnualLeaveTotalDays
    leaveRequest.FinalEmployeeAnnualLeaveTotalDays,                                         // FinalEmployeeAnnualLeaveTotalDays
    leaveRequest.TotalDays,                                                                 // TotalDays

    // --- Sign Dates ---
    employeeSignDate,
    supervisorSignDate,                                                        // SupervisorSignDate (define this value appropriately)
    hrSignDate,                                                                // HrSignDate (define this value appropriately)
    generalDirectorSignDate,                                                   // GeneralDirectorSignDate (define this value appropriately)

    // --- Signatures ---
    isSigned ? "Đã ký" : null,                                                 // EmployeeSignature
    isSigned ? "Đã ký" : null,                                                 // GeneralDirectorSignature
    isSigned ? "Đã ký" : null,                                                 // HrSignature
    isSigned ? "Đã ký" : null,                                                 // SupervisorSignature

    // --- Names (Signers) ---
    hr.FirstName + " " + hr.MiddleName + " " + hr.LastName,                    // HrName
    supervisor.FirstName + " " + supervisor.MiddleName + " " + supervisor.LastName,
    generalDirector.FirstName + " " + generalDirector.MiddleName + " " + generalDirector.LastName
);
        return DocxBookmarkInserter.InsertEmployeeData(props);
    }

}

public class Attachment
{
    public string FileName { get; set; } = "";
    public string ContentType { get; set; } = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
    public byte[] FileContent { get; set; } = Array.Empty<byte>();
}