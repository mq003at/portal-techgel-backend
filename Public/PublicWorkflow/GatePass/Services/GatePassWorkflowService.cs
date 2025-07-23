namespace portal.Services;

using AutoMapper;
using DotNetCore.CAP;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using portal.Db;
using portal.DTOs;
using portal.Enums;
using portal.Extensions;
using portal.Helpers;
using portal.Models;
using Serilog;

public class GatePassWorkflowService
    : BaseWorkflowService<
        GatePassWorkflow,
        GatePassWorkflowDTO,
        GatePassWorkflowCreateDTO,
        GatePassWorkflowUpdateDTO,
        GatePassNode
    >,
        IGatePassWorkflowService
{
    private readonly IFileStorageService _storage;
    private readonly DocumentOptions _docOpts;
    private readonly string _basePath;

    public GatePassWorkflowService(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<GatePassWorkflowService> logger,
        IOptions<DocumentOptions> docOpts,
        IFileStorageService storage
    )
        : base(context, mapper, logger)
    {
        _docOpts = docOpts.Value;
        _storage = storage;
        _basePath = AppDomain.CurrentDomain.BaseDirectory;
    }

    public async Task<List<GatePassNodeDTO>> GenerateNodesAsync(
        GatePassWorkflowCreateDTO dto,
        GatePassWorkflow workflow
    )
    {
        // Get the last node ID (handle empty table)
        var lastId = await _context.GatePassNodes.AnyAsync()
            ? await _context.GatePassNodes.MaxAsync(x => x.Id)
            : 0;

        Employee employee =
            await _context
                .Employees.Include(e => e.Supervisor)
                .Include(e => e.DeputySupervisor)
                .FirstOrDefaultAsync(e => e.Id == workflow.SenderId)
            ?? throw new InvalidOperationException($"Không tìm thấy nhân viên gửi trong hệ thống.");

        Employee supervisor =
            employee.Supervisor
            ?? throw new InvalidOperationException(
                $"Không tìm thấy người quản lý trực tiếp của nhân viên này trong hệ thống."
            );
        Employee? deputySupervisor = employee.DeputySupervisor;

        // Build up receiver IDs for workflow
        // Compose workflow steps


        // Initiate nodes creation: 2 nodes for now, first one has 2 participants, second one has 2 participants
        var steps = new List<(
            string Name,
            GatePassStepType StepType,
            GeneralWorkflowStatusType Status,
            List<WorkflowNodeParticipant> nodeParticipants
        )>
        {
            (
                "Tạo tờ trình chung",
                GatePassStepType.CreateForm,
                GeneralWorkflowStatusType.APPROVED,
                new List<WorkflowNodeParticipant> { }
            ),
            (
                "Người duyệt ký",
                GatePassStepType.ExecutiveApproval,
                GeneralWorkflowStatusType.PENDING,
                new List<WorkflowNodeParticipant> { }
            ),
        };

        // Build and add nodes
        var nodes = new List<GatePassNode>();
        for (int i = 0; i < steps.Count; i++)
        {
            var step = steps[i];

            var node = new GatePassNode
            {
                WorkflowId = workflow.Id,
                StepType = step.StepType,
                MainId = $"PRC-B{i}-{DateTime.Now:dd.MM.yyyy}",
                Name = step.Name,
                Status = (GeneralWorkflowStatusType)step.Status,
                WorkflowNodeParticipants = step.nodeParticipants,
                Description = step.Name,
            };

            nodes.Add(node);
        }
        _context.GatePassNodes.AddRange(nodes);
        await _context.SaveChangesAsync();
        _logger.LogError("Node [0] ID: {id}", nodes[0].Id);

        var participants = new List<(
            string EmpId,
            int WorkflowNodeId,
            GeneralProposalStepType StepType,
            int Order,
            DateTime? ApprovalDate,
            DateTime? ApprovalStartDate,
            DateTime? ApprovalDeadline,
            bool? IsApproved,
            ApprovalStatusType ApprovalStatus,
            TimeSpan? TAT
        )>
        {
            (
                employee.MainId,
                nodes[0].Id,
                GeneralProposalStepType.CreateForm,
                0,
                DateTime.UtcNow,
                DateTime.UtcNow,
                DateTime.UtcNow,
                true,
                ApprovalStatusType.APPROVED,
                TimeSpan.Zero
            ),
            (
                supervisor.MainId,
                nodes[1].Id,
                GeneralProposalStepType.ExecutiveApproval,
                0,
                null,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(48),
                false,
                ApprovalStatusType.PENDING,
                null
            )
        };
        if (deputySupervisor != null)
        {
            participants.Add(
                (
                    deputySupervisor.MainId,
                    nodes[1].Id,
                    GeneralProposalStepType.ExecutiveApproval,
                    1,
                    null,
                    DateTime.UtcNow.AddHours(48),
                    DateTime.UtcNow.AddHours(96),
                    false,
                    ApprovalStatusType.PENDING,
                    null
                )
            );
        }

        // Convert the participants above to WorkflowNodeParticipant objects
        var WorkflowNodeParticipants = new List<WorkflowNodeParticipant>();
        foreach (var participant in participants)
        {
            WorkflowNodeParticipants.Add(
                new WorkflowNodeParticipant
                {
                    EmployeeId =
                        _context.Employees.FirstOrDefault(e => e.MainId == participant.EmpId)?.Id
                        ?? 0,
                    Order = participant.Order,
                    ApprovalDate = participant.ApprovalDate,
                    ApprovalStartDate = participant.ApprovalStartDate,
                    ApprovalDeadline = participant.ApprovalDeadline,
                    ApprovalStatus = participant.ApprovalStatus,
                    TAT = participant.TAT,
                    WorkflowNodeType = "GatePass",
                    WorkflowNodeId = participant.WorkflowNodeId,
                    WorkflowId = workflow.Id
                }
            );

            _logger.LogError(
                "NodeID: {NodeId}, StepType: {StepType}, Order: {Order}, EmployeeId: {EmployeeId}",
                participant.WorkflowNodeId,
                participant.StepType,
                participant.Order,
                participant.EmpId
            );
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

        workflow.ParticipantIds = new List<int>
        {
            WorkflowNodeParticipants[0].EmployeeId,
            WorkflowNodeParticipants[1].EmployeeId,
            WorkflowNodeParticipants[2].EmployeeId
        };
        // Create notification events for all participants
        var employeeIdsToNotify = new List<int> { employee.Id, supervisor.Id };
        employeeIdsToNotify = employeeIdsToNotify.Distinct().ToList();

        _logger.LogError("Employee IDs to notify: {Ids}", string.Join(", ", employeeIdsToNotify));

        var events = new List<CreateEvent>();
        employeeIdsToNotify.ForEach(id =>
        {
            events.Add(
                new CreateEvent
                {
                    WorkflowId = workflow.MainId.ToString(),
                    WorkflowType = "Phiếu ra cổng",
                    EmployeeId = id,
                    ApproverName = supervisor.GetDisplayName(),
                    TriggeredBy = employee.GetDisplayName(),
                    CreatedAt = DateTime.UtcNow,
                    Status = "CREATED",
                    EmployeeName = employee.GetDisplayName(),
                    AssigneeDetails = ""
                }
            );
        });

        foreach (var @event in events)
        {
            _logger.LogError(
                "Publishing event for employee {EmployeeId}: {@Event}",
                @event.EmployeeId,
                @event
            );
            BackgroundJob.Enqueue<WorkflowEventHandler>(handler => handler.HandleCreation(@event));
        }

        await _context.SaveChangesAsync();

        return _mapper.Map<List<GatePassNodeDTO>>(nodes);
    }

    public async Task<IEnumerable<GatePassNodeDTO>> GetNodesByWorkflowIdAsync(int workflowId)
    {
        var nodes = await _context
            .GatePassNodes.Where(n => n.WorkflowId == workflowId)
            .ToListAsync();

        var nodeIds = nodes.Select(n => n.Id).ToList();

        var participants = await _context
            .WorkflowNodeParticipants.Where(p => p.WorkflowNodeType == "GatePass")
            .ToListAsync();

        foreach (var node in nodes)
        {
            node.WorkflowNodeParticipants = participants
                .Where(p => p.WorkflowNodeId == node.Id)
                .ToList();
        }

        var dtos = _mapper.Map<IEnumerable<GatePassNodeDTO>>(nodes);
        _logger.LogInformation(
            "first node has {ParticipantCount} participants.",
            dtos.First().WorkflowNodeParticipants.Count()
        );
        return dtos;
    }

    public async Task<bool> FinalizeIfCompleteAsync(int workflowId)
    {
        GatePassWorkflow workflow =
            _context
                .GatePassWorkflows.Include(wf => wf.GatePassNodes)
                .FirstOrDefault(wf => wf.Id == workflowId)
            ?? throw new KeyNotFoundException(
                $"Không tìm thấy phiếu ra cổng với id: {workflowId}. Có thể đã bị xóa khỏi hệ thống."
            );

        WorkflowNodeParticipant finalParticipant =
            _context
                .Set<WorkflowNodeParticipant>()
                .Where(p =>
                    p.WorkflowId == workflowId
                    && p.WorkflowNodeType == "GatePass"
                    && p.ApprovalStatus == ApprovalStatusType.APPROVED
                )
                .OrderByDescending(p => p.Id)
                .FirstOrDefault()
            ?? throw new InvalidOperationException(
                "Quy trình chưa được phê duyệt hoàn tất, không thể lấy bản vật lý."
            );

        Employee approver =
            await _context
                .Employees.Include(e => e.Signature)
                .Include(e => e.CompanyInfo)
                .FirstOrDefaultAsync(e => e.Id == finalParticipant.EmployeeId)
            ?? throw new InvalidOperationException(
                $"Không tìm thấy người duyệt với ID {finalParticipant.EmployeeId}."
            );

        Employee employee =
            await _context
                .Employees.Include(e => e.Signature)
                .Include(e => e.CompanyInfo)
                .FirstOrDefaultAsync(employee => employee.Id == workflow.SenderId)
            ?? throw new InvalidOperationException(
                $"Không tìm thấy người duyệt với ID {workflow.SenderId}."
            );

        var approverPosition = "";
        if (finalParticipant.EmployeeId == employee.Supervisor?.Id)
            approverPosition = "Supervisor";
        else if (finalParticipant.EmployeeId == employee.DeputySupervisor?.Id)
            approverPosition = "Deputy Supervisor";
        else
            throw new InvalidOperationException(
                $"Có lỗi xảy ra. Không tìm dược người phê duyệt khớp với người quản lý của nhân viên {employee.GetDisplayName()}."
            );

        var result = await GenerateGatePassFinalDocument(
            employee,
            approver,
            workflow,
            approverPosition
        );

        return result;
    }

    public override async Task<GatePassWorkflowDTO> CreateAsync(GatePassWorkflowCreateDTO dto)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Map DTO to entity
            var entity = _mapper.Map<GatePassWorkflow>(dto);
            _logger.LogInformation(
                "Creating GatePassWorkflow with senderId: {SenderId}, startTime: {StartTime}, endTime: {EndTime}",
                entity.SenderId,
                entity.GatePassStartTime,
                entity.GatePassEndTime
            );

            // Save workflow first to get its Id
            _context.GatePassWorkflows.Add(entity);
            await _context.SaveChangesAsync();
            entity.MainId = $"{DateTime.Now:yyyy}/{entity.Id}/PRC";
            entity.Name = $"Phiếu ra cổng số {entity.Id}, năm {DateTime.Now:yyyy}";
            // Generate workflow nodes with entity.Id now available
            var nodes = await GenerateNodesAsync(dto, entity);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            // Map to DTO after commit
            var finalDto = _mapper.Map<GatePassWorkflowDTO>(entity);
            finalDto.GatePassNodes = _mapper.Map<List<GatePassNodeDTO>>(nodes);

            return finalDto;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public override async Task<IEnumerable<GatePassWorkflowDTO>> GetAllAsync()
    {
        _logger.LogError("*************** workflows: " + "ádf");
        var workflows = await _dbSet
            .Include(wf => wf.GatePassNodes)
            .Include(wf => wf.Sender)
            .Include("Sender")
            .ToListAsync();

        var dto = _mapper.Map<IEnumerable<GatePassWorkflowDTO>>(workflows);

        return dto;
    }

    public new async Task<List<GatePassWorkflowDTO>> GetAllByEmployeeIdAsync(int employeeId)
    {
        var workflows = await _context
            .GatePassWorkflows.Where(wf => wf.SenderId == employeeId)
            .Include(wf => wf.Sender)
            .Include(wf => wf.GatePassNodes)
            .ToListAsync();

        return _mapper.Map<List<GatePassWorkflowDTO>>(workflows);
    }

    public override async Task<GatePassWorkflowDTO?> GetByIdAsync(int id)
    {
        // Step 1: Get workflow and its nodes
        GatePassWorkflow workflow =
            await _dbSet.Include(wf => wf.GatePassNodes).FirstOrDefaultAsync(wf => wf.Id == id)
            ?? throw new KeyNotFoundException($"Không tìm thấy phiểu ra cổng với id: {id}.");

        // Step 2: Load all participants in one query
        var nodeIds = workflow.GatePassNodes.Select(n => n.Id).ToList();

        var participants = await _context
            .WorkflowNodeParticipants.Where(p =>
                nodeIds.Contains(p.WorkflowNodeId) && p.WorkflowNodeType == "GatePass"
            )
            .Include(p => p.Employee)
            .ToListAsync();

        // Step 3: Assign participants to nodes
        foreach (var node in workflow.GatePassNodes)
        {
            node.WorkflowNodeParticipants = participants
                .Where(p => p.WorkflowNodeId == node.Id)
                .ToList();
        }

        workflow.DocumentAssociations = await _context
            .DocumentAssociations.Where(da =>
                da.EntityId == workflow.Id && da.EntityType == "GatePass"
            )
            .Include(da => da.Document) // Eager load the Document
            .Select(da => da.Document)
            .ToListAsync();

        var dto = _mapper.Map<GatePassWorkflowDTO>(workflow);
        dto.SenderName =
            workflow.Sender.LastName
            + " "
            + workflow.Sender.MiddleName
            + " "
            + workflow.Sender.FirstName;
        dto.SenderMainId = workflow.Sender.MainId;
        return dto;
    }

    // Add document only at the end of the workflow. The newly created document will be attached to the workflow itself, not the nodes
    public async Task<bool> GenerateGatePassFinalDocument(
        Employee employee,
        Employee approver,
        GatePassWorkflow workflow,
        string approverPosition
    )
    {
        string className = GetType().Name;
        string TemplateKey = className.Split(
            new[] { "Node", "Service", "Workflow" },
            StringSplitOptions.None
        )[0];

        _logger.LogError(
            "Generating final document for workflow {WorkflowId} with template key {TemplateKey}",
            workflow.Id,
            TemplateKey
        );

        var templateDocMetadata = await _context.Documents.FirstOrDefaultAsync(d =>
            d.TemplateKey == TemplateKey
        );

        var today = DateTime.UtcNow;

        if (templateDocMetadata == null)
            throw new InvalidOperationException("Không tìm thấy biểu mẫu tờ trình duyệt chung.");

        // Get that file from the VPS, and all the related signatures
        var newDoc = await _storage.DownloadAsync(templateDocMetadata.Url);

        string employeeSignaturePath =
            employee.Signature?.StoragePath
            ?? throw new InvalidOperationException(
                $"Employee {employee.Id} does not have a signature."
            );
        string approverSignaturePath =
            approver.Signature?.StoragePath
            ?? throw new InvalidOperationException(
                $"Approver {approver.Id} does not have a signature."
            );

        MemoryStream employeeSignature = await FileHandling.ToMemoryStreamAsync(
            await _storage.DownloadAsync(employee.Signature.StoragePath)
        );

        MemoryStream approverSignature = await FileHandling.ToMemoryStreamAsync(
            await _storage.DownloadAsync(approver.Signature.StoragePath)
        );

        bool isImgValid = FileHandling.IsPngHeader(employeeSignature);
        _logger.LogInformation("Employee signature is valid: {IsValid}", isImgValid);

        var Location = "6_Noi_Chinh";
        var newFileName =
            $"{workflow.Id}-{today:yyyy-MM-dd}-{Location}-PRC-{employee.MainId}-v01{".docx"}";
        var newTargetPath = Path.Combine(
                "erp",
                "documents",
                Location,
                "Ho_So",
                "Phieu_Ra_Cong",
                newFileName
            )
            .Replace("\\", "/");

        // Filling in steps
        var placeholders = new Dictionary<string, (string Text, bool IsBold)>
        {
            // English
            ["createdAtDate"] = (workflow.CreatedAt.ToString("dd"), false),
            ["createdAtMonth"] = (workflow.CreatedAt.ToString("MM"), false),
            ["createdAtYear"] = (workflow.CreatedAt.ToString("yyyy"), false),

            ["employeeName"] = (employee.GetDisplayName(), false),
            ["employeePosition"] = (employee.CompanyInfo?.Position ?? "", false),
            ["employeeDepartment"] = (employee.CompanyInfo?.Department ?? "", false),

            ["gatePassDate"] = (workflow.GatePassStartTime.ToString("dd"), false),
            ["gatePassMonth"] = (workflow.GatePassStartTime.ToString("MM"), false),
            ["gatePassYear"] = (workflow.GatePassStartTime.ToString("yyyy"), false),
            ["gatePassStartTime"] = (workflow.GatePassStartTime.ToString("HH:mm"), false),
            ["gatePassEndTime"] = (workflow.GatePassEndTime.ToString("HH:mm"), false),

            ["reason"] = (workflow.Reason, false),

            // Sign Area
            ["employeeSignDate"] = (workflow.CreatedAt.ToString("dd/MM/yyyy"), false),
            ["employeeFullNameBottom"] = (employee.GetDisplayName(), false),

            ["approverPosition"] = (approver.CompanyInfo?.Position ?? "", true),
            ["approverSignDate"] = (today.ToString("dd/MM/yyyy"), false),
            ["approverFullNameBottom"] = (approver.GetDisplayName(), false),
        };

        MemoryStream newMemoryDoc = await FileHandling.ToMemoryStreamAsync(newDoc);

        WordBookmarkReplacer.ReplacePlaceholders(newMemoryDoc, placeholders);

        newMemoryDoc = await WordImageInserter.InsertImageAtBookmarkAsync(
            newMemoryDoc,
            "employeeSignature",
            employeeSignature
        );

        newMemoryDoc = await WordImageInserter.InsertImageAtBookmarkAsync(
            newMemoryDoc,
            "approverSignature",
            approverSignature
        );

        // If filling in is completed, upload and make a new record of metadata
        var pathAfterUpload = await _storage.UploadAsync(newMemoryDoc, newTargetPath);
        if (string.IsNullOrEmpty(pathAfterUpload))
            throw new InvalidOperationException("Tải file lên thất bại. Server bị quá tải.");

        var newMetadata = new Models.Document
        {
            Name = newFileName,
            Url = pathAfterUpload,
            TemplateKey = null,
            Description = $"Phiếu ra cổng của {employee.GetDisplayName()}",
            FileExtension = ".docx",
            SizeInBytes = newDoc.Length,
            Status = DocumentStatusEnum.APPROVED,
            Version = "1.0",
            Tag = templateDocMetadata.Tag,
            Category = DocumentCategoryEnum.PROPOSAL,
            Location = Location
        };

        _logger.LogInformation(System.Text.Json.JsonSerializer.Serialize(workflow.Id));

        DocumentAssociation newDocumentAssociation = new DocumentAssociation
        {
            Document = newMetadata,
            EntityType = TemplateKey,
            EntityId = workflow.Id,
        };

        workflow.IsDocumentGenerated = true;
        _context.Entry(workflow).State = EntityState.Modified;

        _context.Documents.Add(newMetadata);
        _context.DocumentAssociations.Add(newDocumentAssociation);
        await _context.SaveChangesAsync();

        return true;
    }
}
