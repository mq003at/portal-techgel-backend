namespace portal.Services;

using AutoMapper;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using portal.Db;
using portal.DTOs;
using portal.Enums;
using portal.Helpers;
using portal.Models;
using Serilog;

public class LeaveRequestWorkflowService
    : BaseWorkflowService<
        LeaveRequestWorkflow,
        LeaveRequestWorkflowDTO,
        LeaveRequestWorkflowCreateDTO,
        LeaveRequestWorkflowUpdateDTO,
        LeaveRequestNode>,
      ILeaveRequestWorkflowService
{
    private readonly IFileStorageService _storage;
    private readonly DocumentOptions _docOpts;
    private readonly string _basePath;

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
        _basePath = AppDomain.CurrentDomain.BaseDirectory;
    }

    public async Task<List<LeaveRequestNodeDTO>> GenerateNodesAsync(
        LeaveRequestWorkflowCreateDTO dto,
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

        // Fetch organizationally-related details (hr & director)
        var orgIds = new[] { 3, 11 };
        var orgManagerIds = await _context.OrganizationEntities
            .Where(e => orgIds.Contains(e.Id))
            .Select(e => new { e.Id, e.ManagerId })
            .ToListAsync();

        var directorId = orgManagerIds.FirstOrDefault(e => e.Id == 3)?.ManagerId;
        var hrHeadId = orgManagerIds.FirstOrDefault(e => e.Id == 11)?.ManagerId;

        // Fetch other participants (sender, emp, assignee, manager)

        var employeeId = dto.EmployeeId;
        var assigneeId = dto.AssigneeId;
        var senderId = dto.SenderId;

        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == employeeId);

        var managerId = employee?.SupervisorId;

        var employeeIds = new[] {
            employeeId,
            assigneeId,
            senderId,
            directorId,
            hrHeadId,
            managerId
        }.Where(id => id.HasValue)
        .Select(id => id!.Value)
        .Distinct()
        .ToList();

        var employeeIdInts = employeeIds.Select(id => (int)id).ToList();

        var employeeDict = await _context.Employees
            .Where(e => employeeIdInts.Contains(e.Id))
            .ToDictionaryAsync(e => e.Id);

        employee = employeeDict[employeeId];
        var assignee = employeeDict[assigneeId];
        var sender = employeeDict[senderId];
        var director = directorId.HasValue ? employeeDict[directorId.Value] : null;
        var hrHead = hrHeadId.HasValue ? employeeDict[hrHeadId.Value] : null;
        var manager = managerId.HasValue ? employeeDict[managerId.Value] : null;

        var isOnProbation = employee.CompanyInfo.IsOnProbation;

        if (employee == null)
            throw new InvalidOperationException($"Employee {dto.EmployeeId} not found.");

        // Building Leave information using Employee navigation properties
        var finalEmployeeAnnualLeaveTotalDays = employee.CompanyInfo.AnnualLeaveTotalDays - (double)totalDays;

        workflow.TotalDays = (double)totalDays;
        workflow.EmployeeAnnualLeaveTotalDays = employee.CompanyInfo.AnnualLeaveTotalDays;
        workflow.FinalEmployeeAnnualLeaveTotalDays = finalEmployeeAnnualLeaveTotalDays;

        if (manager == null || hrHead == null || director == null)
            throw new InvalidOperationException($"Manager for employee {dto.EmployeeId} not found.");

        // Build up receiver IDs for workflow
        // Compose workflow steps
        var participants = new List<(string EmpId, int WorkflowNodeId, LeaveApprovalStepType StepType, int Order, DateTime? ApprovalDate, DateTime? ApprovalDeadline, bool? IsApproved, bool? HasApproved, bool? HasRejected, TimeSpan? TAT)>
        {
            (employee.MainId, 0, LeaveApprovalStepType.CreateForm, 0, DateTime.UtcNow, DateTime.UtcNow, true, true, false, TimeSpan.Zero),
            (sender.MainId, 0, LeaveApprovalStepType.CreateForm, 0, null, null, false, false, false, null),
            (assignee.MainId, 0, LeaveApprovalStepType.CreateForm, 0, null, null, false, false, false, null),
            (manager.MainId, 1, LeaveApprovalStepType.ManagerApproval, 0, null, DateTime.UtcNow.AddDays(3), false, false, false, null),
            (hrHead.MainId, 2, LeaveApprovalStepType.HRHeadApproval, 0, null, null, false, false, false, null),
            (director.MainId, 3, LeaveApprovalStepType.ExecutiveApproval, 0, null, null, false, false, false, null)
        };

        // Convert the participants above to WorkflowNodeParticipant objects
        var workflowParticipants = new List<WorkflowNodeParticipant>();
        foreach (var participant in participants)
        {
            workflowParticipants.Add(new WorkflowNodeParticipant
            {
                EmployeeId = _context.Employees.FirstOrDefault(e => e.MainId == participant.EmpId)?.Id ?? 0,
                WorkflowNodeStepType = (int)participant.StepType,
                Order = participant.Order,
                ApprovalDate = participant.ApprovalDate,
                ApprovalDeadline = participant.ApprovalDeadline,
                HasApproved = participant.IsApproved,
                HasRejected = participant.HasRejected,
                TAT = participant.TAT
            });
        }
        _context.WorkflowNodeParticipants.AddRange(workflowParticipants);

        // Initiate nodes creation: 
        var steps = new List<(string Name, LeaveApprovalStepType StepType, int Status, List<WorkflowNodeParticipant> nodeParticipants, List<int> documentAssocation)>
        {
            ("Tạo yêu cầu nghỉ phép", LeaveApprovalStepType.CreateForm, 1, new List<WorkflowNodeParticipant> { workflowParticipants[0], workflowParticipants[1], workflowParticipants[2] }, new List<int>()),
            ("Quản lý trực thuộc ký", LeaveApprovalStepType.ManagerApproval, 0, new List<WorkflowNodeParticipant> { workflowParticipants[3] }, new List<int>()),
            ("Trưởng phòng nhân sự ký", LeaveApprovalStepType.HRHeadApproval, 2, new List<WorkflowNodeParticipant> { workflowParticipants[4] }, new List<int>()),
            ("Ký xác nhận cuối cùng", LeaveApprovalStepType.ExecutiveApproval, 2, new List<WorkflowNodeParticipant> { workflowParticipants[5] }, new List<int>())
        };

        // Build and add nodes
        var nodes = new List<LeaveRequestNode>();
        for (int i = 0; i < steps.Count; i++)
        {
            var step = steps[i];
            var newId = lastId + i + 1;

            var node = new LeaveRequestNode
            {
                WorkflowId = workflow.Id,
                StepType = step.StepType,
                MainId = $"AS-S{i}-{DateTime.Now:dd.MM.yyyy}/{newId}",
                Name = step.Name,
                Status = (GeneralWorkflowStatusType)step.Status,
                WorkflowParticipants = step.nodeParticipants,
                Description = step.Name,
            };
            
            _logger.LogError("Created leave request node: {NodeName} in workflow: {WorkflowId}", node.Name, workflow.Id);

            nodes.Add(node);

        }
        _context.LeaveRequestNodes.AddRange(nodes);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created leave request workflow for manager: {ManagerName}, HR: {HRName}",
            manager.FirstName, hrHead.FirstName);

        // Add documnent associations 
        // var copiedDocument = await GenerateLeaveRequestInitDocument(
        //     employee,
        //     assignee,
        //     sender,
        //     hrHead,
        //     director,
        //     manager,
        //     dto,
        //     totalDays,
        //     finalEmployeeAnnualLeaveTotalDays
        // );

        // // Step 4: Attach to final approval step nodes
        // var firstNode = nodes.FirstOrDefault();
        // if (firstNode != null)
        // {
        //     firstNode.DocumentAssociations.Add(new DocumentAssociation
        //     {
        //         DocumentId = copiedDocument.Id,
        //         EntityType = nameof(LeaveRequestNode)
        //     });
        // }

        _context.LeaveRequestWorkflows.Update(workflow);
        await _context.SaveChangesAsync();
        return _mapper.Map<List<LeaveRequestNodeDTO>>(nodes);
    }



    public async Task<IEnumerable<LeaveRequestNodeDTO>> GetNodesByWorkflowIdAsync(int workflowId)
    {
        var nodes = await _context.LeaveRequestNodes
            .Where(n => n.WorkflowId == workflowId)
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
        LeaveRequestWorkflowCreateDTO dto
    )
    {
        // CDTO -> Model
        var entity = _mapper.Map<LeaveRequestWorkflow>(dto);

        // Generate Id + timestamp + roles
        _context.LeaveRequestWorkflows.Add(entity);
        await _context.SaveChangesAsync();

        // Generate nodes based on the workflow. We already have roles from tables
        var nodes = await GenerateNodesAsync(dto, entity);

        var finalDto = _mapper.Map<LeaveRequestWorkflowDTO>(entity);
        _logger.LogError(
            "Generated {NodeCount} employeeAnualLeave for this {WorkflowId} in dto",
            entity.EmployeeAnnualLeaveTotalDays,
            finalDto.EmployeeAnnualLeaveTotalDays
        );
        // Populate thêm metadata nếu cần
        finalDto.LeaveRequestNodes = nodes;

        return finalDto;
    }

    public override async Task<LeaveRequestWorkflowDTO?> UpdateAsync(
        int id,
        LeaveRequestWorkflowUpdateDTO dto
    )
    {
        var workflow = await base.UpdateAsync(id, dto);

        if (workflow == null)
            return null;
        // Populate metadata after update
        return workflow;
    }

    public override async Task<LeaveRequestWorkflowDTO?> GetByIdAsync(int id)
    {
        var workflow = await base.GetByIdAsync(id);

        return workflow;
    }

    public override async Task<IEnumerable<LeaveRequestWorkflowDTO>> GetAllAsync()
    {
        var workflows = await base.GetAllAsync();
        var workflowList = workflows.ToList();

        return workflowList;
    }
    private async Task<List<string>> GetNamesByIdsAsync(List<int> ids)
    {
        return await _context.Employees
            .Where(e => ids.Contains(e.Id))
            .Select(e => e.LastName + " " + e.MiddleName + " " + e.FirstName)
            .ToListAsync();
    }

    // Add document only at the end of the workflow. The newly created document will be attached to the workflow itself, not the nodes
    public async Task<Models.Document> GenerateLeaveRequestInitDocument(
    Employee employee,
    Employee assignee,
    Employee sender,
    Employee hr,
    Employee ceo,
    Employee supervisor,
    LeaveRequestWorkflowCreateDTO dto,
    double totalDays, double finalEmployeeAnnualLeaveTotalDays)
    {
        var templateDocMetadata = await _context.Documents
            .FirstOrDefaultAsync(d => d.TemplateKey == "LeaveRequest");

        var today = DateTime.UtcNow;

        if (templateDocMetadata == null)
            throw new InvalidOperationException("Template document for 'LeaveRequest' not found.");

        // Get that file from the VPS
        var newDoc = await _storage.DownloadAsync(templateDocMetadata.Url);

        var Division = "6_Noi_Chinh";
        var newFileName = $"{today:yyyy-MM-dd}-{Division}-DN-{employee.MainId}-v01{".docx"}";
        var newTargetPath = Path.Combine("erp", "documents", Division, "Ho_So", "Nghi_Phep", newFileName).Replace("\\", "/");

        _logger.LogError(hr.FirstName, supervisor.FirstName, ceo.FirstName);

        // Filling in steps
        var placeholders = new Dictionary<string, string>
        {
            // English
            ["fullName"] = $"{employee.LastName} {employee.MiddleName} {employee.FirstName}".Trim(),
            ["department"] = employee.CompanyInfo.Department ?? "",
            ["position"] = employee.CompanyInfo.Position ?? "",
            ["startDate"] = dto.StartDate.ToString("dd/MM/yyyy"),
            ["endDate"] = dto.EndDate.ToString("dd/MM/yyyy"),
            ["reason"] = dto.Reason ?? "",
            ["leaveRequestStartDate"] = dto.StartDate.ToString("dd/MM/yyyy"),
            ["leaveRequestEndDate"] = dto.EndDate.ToString("dd/MM/yyyy"),
            ["totalDaysTop"] = totalDays.ToString(),
            ["totalDaysBox"] = totalDays.ToString(),
            ["finalAnnualTotal"] = finalEmployeeAnnualLeaveTotalDays.ToString(),


            ["employeeName"] = $"{employee.LastName} {employee.MiddleName} {employee.FirstName}".Trim(),
            ["assigneeFullname"] = $"{assignee.LastName} {assignee.MiddleName} {assignee.FirstName}".Trim() +
                (assignee.Id == employee.Id ? " (Vẫn trực khi nghỉ)" : ""),
            ["assigneePhoneNumber"] = assignee.PersonalInfo.PersonalPhoneNumber ?? "",
            ["assigneeEmail"] = assignee.PersonalInfo.PersonalEmail ?? "",
            ["assigneeAddress"] = assignee.PersonalInfo.Address ?? "",
            ["employeeSignDate"] = today.ToString("dd/MM/yyyy"),

            ["empAnnualTotal"] = employee.CompanyInfo.AnnualLeaveTotalDays.ToString(),
            ["employeeFinalTotalDays"] = finalEmployeeAnnualLeaveTotalDays.ToString(),

            ["employeeFullName"] = $"{employee.LastName} {employee.MiddleName} {employee.FirstName}".Trim(),
            ["employeeFullNameBottom"] = $"{employee.LastName} {employee.MiddleName} {employee.FirstName}".Trim(),

            ["supervisorFullName"] = $"{supervisor.LastName} {supervisor.MiddleName} {supervisor.FirstName}".Trim(),

            ["supervisorFullName"] = $"{supervisor.LastName} {supervisor.MiddleName} {supervisor.FirstName}".Trim(),
            ["hrFullName"] = $"{hr.LastName} {hr.MiddleName} {hr.FirstName}".Trim(),
            ["ceoFullName"] = $"{ceo.LastName} {ceo.MiddleName} {ceo.FirstName}".Trim(),

            // Custom: If assigneeId == employeeId, add a special note

            // Vietnamese for LeaveAprrovalCategory
            ["type"] = dto.LeaveAprrovalCategory switch
            {
                LeaveAprrovalCategory.AnnualLeave => "Nghỉ phép có lương",
                LeaveAprrovalCategory.UnpaidLeave => "Nghỉ phép không lương",
                LeaveAprrovalCategory.SickLeave => "Nghỉ ốm",
                LeaveAprrovalCategory.MaternityLeave => "Nghỉ thai sản",
                LeaveAprrovalCategory.PaternityLeave => "Nghỉ tang",
                _ => dto.LeaveAprrovalCategory.ToString()
            },

        };

        WordBookmarkReplacer.ReplacePlaceholders(newDoc, placeholders);

        // If filling in is completed, upload and make a new record of metadata
        var pathAfterUpload = await _storage.UploadAsync(newDoc, newTargetPath);
        if (string.IsNullOrEmpty(pathAfterUpload))
            throw new InvalidOperationException("Failed to upload the document to storage.");

        var newMetadata = new Models.Document
        {
            Name = newFileName,
            Url = pathAfterUpload,
            TemplateKey = null,
            Description = $"Mẫu đơn nghỉ phép của {employee.LastName} {employee.MiddleName} {employee.FirstName}",
            FileExtension = ".docx",
            SizeInBytes = newDoc.Length,
            Status = DocumentStatusEnum.DRAFT,
            Version = "1.0",
            Tag = templateDocMetadata.Tag,
            Category = DocumentCategoryEnum.FORM,
            Division = Division
        };

        _context.Documents.Add(newMetadata);
        await _context.SaveChangesAsync();

        return newMetadata;
    }

    public class Attachment
    {
        public string FileName { get; set; } = "";
        public string ContentType { get; set; } = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        public byte[] FileContent { get; set; } = Array.Empty<byte>();
    }
}