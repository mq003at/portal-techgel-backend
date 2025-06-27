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

        var employeeId = dto.EmployeeId;
        var senderId = dto.SenderId;
        Employee employee = await _context.Employees.Include(e => e.CompanyInfo).Include(e => e.ScheduleInfo).Include(e => e.Supervisor).Include(e => e.DeputySupervisor).FirstOrDefaultAsync(e => e.Id == employeeId) ?? throw new InvalidOperationException($"Employee {dto.EmployeeId} not found.");

        // Null checks for employee properties
        CompanyInfo companyInfo = employee.CompanyInfo ?? throw new InvalidOperationException($"CompanyInfo for employee {dto.EmployeeId} not found.");
        Employee supervisor = employee.Supervisor ?? throw new InvalidOperationException($"Supervisor for employee {dto.EmployeeId} not found.");
        Employee deputySupervisor = employee.DeputySupervisor ?? throw new InvalidOperationException($"Deputy Supervisor for employee {dto.EmployeeId} not found.");
        // ScheduleInfo scheduleInfo = employee.ScheduleInfo ?? throw new InvalidOperationException($"ScheduleInfo for employee {dto.EmployeeId} not found.");


        // IF isOnProbation, DO NOT reduce AnnualLeave or CompensatoryLeave
        var isOnProbation = companyInfo.IsOnProbation;
        if (isOnProbation && (dto.LeaveApprovalCategory == LeaveApprovalCategory.AnnualLeave || dto.LeaveApprovalCategory == LeaveApprovalCategory.CompensatoryLeave))
        {
            throw new InvalidOperationException("Cannot request Annual or Compensatory leave while on probation.");
        }


        // Building Leave information using Employee navigation properties
        var compensatoryLeaveInit = companyInfo.CompensatoryLeaveTotalDays;
        var annualLeaveInit = companyInfo.AnnualLeaveTotalDays;
        var finalEmployeeAnnualLeaveTotalDays = annualLeaveInit;
        var finalEmployeeCompensatoryLeaveTotalDays = compensatoryLeaveInit;

        workflow.TotalDays = (double)totalDays;

        // calculate for the 2 specific leaves
        if (dto.LeaveApprovalCategory == LeaveApprovalCategory.CompensatoryLeave)
        {
            if (compensatoryLeaveInit < totalDays)
            {
                throw new InvalidOperationException("Not enough compensatory leave days available. Contact admins to see if anything is wrong.");
            }
            finalEmployeeCompensatoryLeaveTotalDays = compensatoryLeaveInit - totalDays;
            _logger.LogError("finalDays: {fdays}", finalEmployeeCompensatoryLeaveTotalDays);
        }
        else if (dto.LeaveApprovalCategory == LeaveApprovalCategory.AnnualLeave)
        {
            if (annualLeaveInit < totalDays)
            {
                throw new InvalidOperationException("Not enough annual leave days available. Contact admins to see if anything is wrong.");
            }
            finalEmployeeAnnualLeaveTotalDays = annualLeaveInit - totalDays;
            _logger.LogError("finalDays: {fdays}", finalEmployeeAnnualLeaveTotalDays);

        }

        // For other types of leave, we do not modify annual leave or compensatory leave
        workflow.FinalEmployeeCompensatoryLeaveTotalDays = finalEmployeeCompensatoryLeaveTotalDays;
        workflow.FinalEmployeeAnnualLeaveTotalDays = finalEmployeeAnnualLeaveTotalDays;
        workflow.EmployeeCompensatoryLeaveTotalDays = compensatoryLeaveInit;
        workflow.EmployeeAnnualLeaveTotalDays = annualLeaveInit;
        workflow.EmployeeId = employeeId;
        workflow.Reason = dto.Reason;
        workflow.LeaveApprovalCategory = dto.LeaveApprovalCategory;
        workflow.Employee = employee;

        // Build up receiver IDs for workflow
        // Compose workflow steps


        // Initiate nodes creation: 2 nodes for now, first one has 2 participants, second one has 2 participants
        var steps = new List<(string Name, LeaveApprovalStepType StepType, int Status, List<WorkflowNodeParticipant> nodeParticipants)>
        {
            ("Tạo yêu cầu nghỉ phép", LeaveApprovalStepType.CreateForm, 1, new List<WorkflowNodeParticipant> {}),
            ("Quản lý trực thuộc / gián tiếp ký", LeaveApprovalStepType.ExecutiveApproval, 0, new List<WorkflowNodeParticipant> {}),
        };

        // Build and add nodes
        var nodes = new List<LeaveRequestNode>();
        for (int i = 0; i < steps.Count; i++)
        {
            var step = steps[i];

            var node = new LeaveRequestNode
            {
                WorkflowId = workflow.Id,
                StepType = step.StepType,
                MainId = $"AS-S{i}-{DateTime.Now:dd.MM.yyyy}",
                Name = step.Name,
                Status = (GeneralWorkflowStatusType)step.Status,
                WorkflowNodeParticipants = step.nodeParticipants,
                Description = step.Name,
            };

            nodes.Add(node);

        }
        _context.LeaveRequestNodes.AddRange(nodes);
        await _context.SaveChangesAsync();
        _logger.LogError("Node [0] ID: {id}", nodes[0].Id);

        var participants = new List<(string EmpId, int WorkflowNodeId, LeaveApprovalStepType StepType, int Order, DateTime? ApprovalDate, DateTime? ApprovalStartDate, DateTime? ApprovalDeadline, bool? IsApproved, ApprovalStatusType ApprovalStatus, TimeSpan? TAT)>
        {
            (employee.MainId, nodes[0].Id, LeaveApprovalStepType.CreateForm, 0, DateTime.UtcNow, DateTime.UtcNow, DateTime.UtcNow, true, ApprovalStatusType.APPROVED, TimeSpan.Zero),
            (supervisor.MainId, nodes[1].Id , LeaveApprovalStepType.ExecutiveApproval, 0, null, DateTime.UtcNow, DateTime.UtcNow.AddHours(48), false, ApprovalStatusType.PENDING, null),
            (deputySupervisor.MainId, nodes[1].Id, LeaveApprovalStepType.ExecutiveApproval, 1, null, DateTime.UtcNow.AddHours(48), DateTime.UtcNow.AddHours(96), false, ApprovalStatusType.PENDING, null),
        };

        // Convert the participants above to WorkflowNodeParticipant objects
        var WorkflowNodeParticipants = new List<WorkflowNodeParticipant>();
        foreach (var participant in participants)
        {
            WorkflowNodeParticipants.Add(new WorkflowNodeParticipant
            {
                EmployeeId = _context.Employees.FirstOrDefault(e => e.MainId == participant.EmpId)?.Id ?? 0,
                WorkflowNodeStepType = (int)participant.StepType,
                Order = participant.Order,
                ApprovalDate = participant.ApprovalDate,
                ApprovalStartDate = participant.ApprovalStartDate,
                ApprovalDeadline = participant.ApprovalDeadline,
                ApprovalStatus = participant.ApprovalStatus,
                TAT = participant.TAT,
                WorkflowNodeType = "LeaveRequest",
                WorkflowNodeId = participant.WorkflowNodeId
            });

            _logger.LogError("NodeID: {NodeId}, StepType: {StepType}, Order: {Order}, EmployeeId: {EmployeeId}",
                participant.WorkflowNodeId, participant.StepType, participant.Order, participant.EmpId);
        }
        _context.WorkflowNodeParticipants.AddRange(WorkflowNodeParticipants);

        nodes[0].WorkflowNodeParticipants = new List<WorkflowNodeParticipant>
        {
            WorkflowNodeParticipants[0] // Employee who created the request
        };
        nodes[1].WorkflowNodeParticipants = new List<WorkflowNodeParticipant>
        {
            WorkflowNodeParticipants[1],
            WorkflowNodeParticipants[2]
        };
        await _context.SaveChangesAsync();

        return _mapper.Map<List<LeaveRequestNodeDTO>>(nodes);
    }



    public async Task<IEnumerable<LeaveRequestNodeDTO>> GetNodesByWorkflowIdAsync(int workflowId)
    {
        var nodes = await _context.LeaveRequestNodes
            .Where(n => n.WorkflowId == workflowId)
            .ToListAsync();

        var nodeIds = nodes.Select(n => n.Id).ToList();

        var participants = await _context.WorkflowNodeParticipants
            .Where(p => p.WorkflowNodeType == "LeaveRequest")
            .ToListAsync();


        foreach (var node in nodes)
        {
            node.WorkflowNodeParticipants = participants.Where(p => p.WorkflowNodeId == node.Id).ToList();
        }

        var dtos = _mapper.Map<IEnumerable<LeaveRequestNodeDTO>>(nodes);
        _logger.LogInformation("first node has {ParticipantCount} participants.", dtos.First().WorkflowNodeParticipants.Count());
        return dtos;
    }

    public async Task<bool> FinalizeIfCompleteAsync(LeaveRequestWorkflow workflow, int approverId, int nodeId)
    {
        Employee employee = await _context.Employees
            .Include(e => e.CompanyInfo)
            .Include(e => e.Signature)
            .FirstOrDefaultAsync(e => e.Id == workflow.EmployeeId) ?? throw new InvalidOperationException($"Employee {workflow.EmployeeId} not found.");

        Employee approver = await _context.Employees
            .Include(e => e.CompanyInfo)
            .Include(e => e.Signature)
            .FirstOrDefaultAsync(e => e.Id == approverId) ?? throw new InvalidOperationException($"Employee {approverId} not found.");


        return await GenerateLeaveRequestFinalDocument(employee, approver, workflow, nodeId);
    }



    public override async Task<LeaveRequestWorkflowDTO> CreateAsync(
        LeaveRequestWorkflowCreateDTO dto
    )
    {
        // check if the employee already has a workflow in pending state
        var existingWorkflow = await _context.LeaveRequestWorkflows
            .Where(w => w.EmployeeId == dto.EmployeeId && w.Status == GeneralWorkflowStatusType.PENDING)
            .FirstOrDefaultAsync();

        if (existingWorkflow != null)
        {
            throw new InvalidOperationException("You already have a pending leave request. Please wait for it to be processed before creating a new one.");
        }

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
        // Step 1: Get workflow and its nodes
        LeaveRequestWorkflow workflow = await _dbSet
            .Include(wf => wf.LeaveRequestNodes)
            .FirstOrDefaultAsync(wf => wf.Id == id)
            ?? throw new KeyNotFoundException($"Workflow with ID {id} not found.");

        // Step 2: Load all participants in one query
        var nodeIds = workflow.LeaveRequestNodes.Select(n => n.Id).ToList();

        var participants = await _context.WorkflowNodeParticipants
            .Where(p => nodeIds.Contains(p.WorkflowNodeId) && p.WorkflowNodeType == "LeaveRequest")
            .ToListAsync();

        // Step 3: Assign participants to nodes
        foreach (var node in workflow.LeaveRequestNodes)
        {
            node.WorkflowNodeParticipants = participants
                .Where(p => p.WorkflowNodeId == node.Id)
                .ToList();
        }

        workflow.DocumentAssociations = await _context.DocumentAssociations
            .Where(da => da.WorkflowId == workflow.Id && da.EntityType == "LeaveRequest")
            .Include(da => da.Document) // Eager load the Document
            .Select(da => da.Document)
            .ToListAsync();

        return _mapper.Map<LeaveRequestWorkflowDTO>(workflow);
    }

    public async Task<LeaveRequestNodeDTO?> GetAllWorkflowByEmployeeId(int employeeId)
    {
        var workflow = await _context.LeaveRequestWorkflows
            .Include(w => w.LeaveRequestNodes)
            .FirstOrDefaultAsync(w => w.EmployeeId == employeeId);

        if (workflow == null)
            return null;

        var workflowDto = _mapper.Map<LeaveRequestNodeDTO>(workflow);
        return workflowDto;
    }


    public override async Task<IEnumerable<LeaveRequestWorkflowDTO>> GetAllAsync()
    {
        var workflows = await base.GetAllAsync();
        var workflowList = workflows.ToList();

        return workflowList;
    }

    private async Task<string> GenerateDocumentsAsync(int workflowId, int approverId)
    {
        LeaveRequestWorkflow workflow = await _context.LeaveRequestWorkflows
            .Include(w => w.LeaveRequestNodes)
            .FirstOrDefaultAsync(w => w.Id == workflowId) ?? throw new InvalidOperationException($"Workflow {workflowId} not found.");

        // Get the approver's information
        Employee approver = await _context.Employees.FindAsync(approverId) ?? throw new InvalidOperationException($"Approver {approverId} not found.");
        // Get the employee's information
        Employee employee = await _context.Employees.FindAsync(workflow.EmployeeId) ?? throw new InvalidOperationException($"Employee {workflow.EmployeeId} not found.");
        return "";
    }

    private async Task<List<string>> GetNamesByIdsAsync(List<int> ids)
    {
        return await _context.Employees
            .Where(e => ids.Contains(e.Id))
            .Select(e => e.LastName + " " + e.MiddleName + " " + e.FirstName)
            .ToListAsync();
    }

    // Add document only at the end of the workflow. The newly created document will be attached to the workflow itself, not the nodes
    public async Task<bool> GenerateLeaveRequestFinalDocument(
    Employee employee,
    Employee approver,
    LeaveRequestWorkflow workflow,
    int nodeId
    )
    {
        var templateDocMetadata = await _context.Documents
            .FirstOrDefaultAsync(d => d.TemplateKey == "LeaveRequest");

        var today = DateTime.UtcNow;

        if (templateDocMetadata == null)
            throw new InvalidOperationException("Template document for 'LeaveRequest' not found.");

        // Get that file from the VPS, and all the related signatures
        var newDoc = await _storage.DownloadAsync(templateDocMetadata.Url);

        string employeeSignaturePath = employee.Signature?.StoragePath ?? throw new InvalidOperationException($"Employee {employee.Id} does not have a signature.");
        string approverSignaturePath = approver.Signature?.StoragePath ?? throw new InvalidOperationException($"Approver {approver.Id} does not have a signature.");

        MemoryStream employeeSignature = await FileHandling.ToMemoryStreamAsync(
            await _storage.DownloadAsync(employee.Signature.StoragePath));

        MemoryStream approverSignature = await FileHandling.ToMemoryStreamAsync(
            await _storage.DownloadAsync(approver.Signature.StoragePath));

        bool isImgValid = FileHandling.IsPngHeader(employeeSignature);
        _logger.LogInformation("Employee signature is valid: {IsValid}", isImgValid);

        var Division = "6_Noi_Chinh";
        var newFileName = $"{today:yyyy-MM-dd}-{Division}-DN-{employee.MainId}-v01{".docx"}";
        var newTargetPath = Path.Combine("erp", "documents", Division, "Ho_So", "Nghi_Phep", newFileName).Replace("\\", "/");

        // Filling in steps
        var placeholders = new Dictionary<string, (string Text, bool IsBold)>
        {
            // English
            ["fullName"] = ($"{employee.LastName} {employee.MiddleName} {employee.FirstName}".Trim(), false),
            ["department"] = (employee.CompanyInfo?.Department ?? "", false),
            ["position"] = (employee.CompanyInfo?.Position ?? "", false),
            ["startDate"] = (workflow.StartDate.ToString("dd/MM/yyyy"), false),
            ["endDate"] = (workflow.EndDate.ToString("dd/MM/yyyy"), false),
            ["reason"] = (workflow.Reason ?? "", false),
            ["leaveRequestStartDate"] = (
         (workflow.StartDateDayNightType == 0 ? "Sáng " : "Chiều ") + workflow.StartDate.ToString("dd/MM/yyyy"),
         false
     ),
            ["leaveRequestEndDate"] = (
         (workflow.EndDateDayNightType == 0 ? "Sáng " : "Chiều ") + workflow.EndDate.ToString("dd/MM/yyyy"),
         false
     ),
            ["totalDaysTop"] = (workflow.TotalDays.ToString(), false),
            ["totalDaysBox"] = (workflow.TotalDays.ToString(), false),
            ["finalAnnualTotal"] = (workflow.FinalEmployeeAnnualLeaveTotalDays.ToString(), false),

            ["employeeName"] = ($"{employee.LastName} {employee.MiddleName} {employee.FirstName}".Trim(), false),
            ["employeeId"] = (employee.Id.ToString(), false),
            ["employeeSignDate"] = (today.ToString("dd/MM/yyyy"), false),
            ["assigneeDetails"] = (workflow.AssigneeDetails, false),
            ["notes"] = (workflow.Notes ?? "", false),

            ["empAnnualTotal"] = (employee.CompanyInfo?.AnnualLeaveTotalDays.ToString() ?? "", false),
            ["empCompensatoryTotal"] = (employee.CompanyInfo?.CompensatoryLeaveTotalDays.ToString() ?? "", false),
            ["totalAnnualDays"] = (workflow.TotalDays.ToString(), false),
            ["finalCompensatoryTotal"] = (workflow.FinalEmployeeCompensatoryLeaveTotalDays.ToString(), false),

            ["employeeFullName"] = ($"{employee.LastName} {employee.MiddleName} {employee.FirstName}".Trim(), false),
            ["employeeFullNameBottom"] = ($"{employee.LastName} {employee.MiddleName} {employee.FirstName}".Trim(), false),
            ["employeeSignDate"] = (workflow.CreatedAt.ToString("dd/MM/yyyy"), false),

            ["approverPosition"] = (approver.CompanyInfo?.Position?.ToUpper() ?? "", true), // BOLD
            ["approverFullName"] = ($"{approver.LastName} {approver.MiddleName} {approver.FirstName}".Trim(), false),
            ["approverSignDate"] = (today.ToString("dd/MM/yyyy"), false),

            ["type"] = (workflow.LeaveApprovalCategory switch
            {
                LeaveApprovalCategory.AnnualLeave => "Nghỉ phép có lương",
                LeaveApprovalCategory.UnpaidLeave => "Nghỉ phép không lương",
                LeaveApprovalCategory.SickLeave => "Nghỉ ốm",
                LeaveApprovalCategory.MaternityLeave => "Nghỉ thai sản",
                LeaveApprovalCategory.PaternityLeave => "Nghỉ tang",
                LeaveApprovalCategory.CompensatoryLeave => "Nghỉ bù",
                _ => workflow.LeaveApprovalCategory.ToString()
            }, false)
        };

        MemoryStream newMemoryDoc = await FileHandling.ToMemoryStreamAsync(
            newDoc);

        WordBookmarkReplacer.ReplacePlaceholders(newMemoryDoc, placeholders);

        newMemoryDoc = await WordImageInserter.InsertImageAtBookmarkAsync(
            newMemoryDoc, "employeeSignature", employeeSignature);

        newMemoryDoc = await WordImageInserter.InsertImageAtBookmarkAsync(
            newMemoryDoc, "approverSignature", approverSignature);

        // If filling in is completed, upload and make a new record of metadata
        var pathAfterUpload = await _storage.UploadAsync(newMemoryDoc, newTargetPath);
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

        DocumentAssociation newDocumentAssociation = new DocumentAssociation
        {
            Document = newMetadata,
            EntityType = "LeaveRequest",
            EntityId = nodeId, // Associate with the workflow ID
            WorkflowId = workflow.Id
        };

        _context.Documents.Add(newMetadata);
        _context.DocumentAssociations.Add(newDocumentAssociation);
        await _context.SaveChangesAsync();

        return true;
    }

    public class Attachment
    {
        public string FileName { get; set; } = "";
        public string ContentType { get; set; } = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        public byte[] FileContent { get; set; } = Array.Empty<byte>();
    }
}