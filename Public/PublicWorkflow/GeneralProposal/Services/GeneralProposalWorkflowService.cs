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

public class GeneralProposalWorkflowService
    : BaseWorkflowService<
        GeneralProposalWorkflow,
        GeneralProposalWorkflowDTO,
        GeneralProposalWorkflowCreateDTO,
        GeneralProposalWorkflowUpdateDTO,
        GeneralProposalNode
    >,
        IGeneralProposalWorkflowService
{
    private readonly IFileStorageService _storage;
    private readonly DocumentOptions _docOpts;
    private readonly string _basePath;

    public GeneralProposalWorkflowService(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<GeneralProposalWorkflowService> logger,
        IOptions<DocumentOptions> docOpts,
        IFileStorageService storage
    )
        : base(context, mapper, logger)
    {
        _docOpts = docOpts.Value;
        _storage = storage;
        _basePath = AppDomain.CurrentDomain.BaseDirectory;
    }

    public new async Task<List<GeneralProposalWorkflowDTO>> GetAllByEmployeeIdAsync(int employeeId)
    {
        var workflows = await _context.GeneralProposalWorkflows
            .Where(wf => wf.SenderId == employeeId)
            .Include(wf => wf.Sender)
            .Include(wf => wf.GeneralProposalNodes)
            .ToListAsync();

        return _mapper.Map<List<GeneralProposalWorkflowDTO>>(workflows);
    }

    public override async Task<IEnumerable<GeneralProposalWorkflowDTO>> GetAllAsync()
    {
        var workflows = await _dbSet
            .Include(wf => wf.GeneralProposalNodes)
            .Include(wf => wf.Sender)
            .ToListAsync();

        var workflowIds = workflows.Select(wf => wf.Id).ToList();

        var allAssociations = await _context.DocumentAssociations
            .Where(da => workflowIds.Contains(da.EntityId) && da.EntityType == "GeneralProposal")
            .Include(da => da.Document)
            .ToListAsync();

        foreach (var wf in workflows)
        {
            wf.DocumentAssociations = allAssociations
                .Where(da => da.EntityId == wf.Id)
                .Select(da => da.Document)
                .ToList();
        }

        return _mapper.Map<IEnumerable<GeneralProposalWorkflowDTO>>(workflows);
    }
    

    public async Task<List<GeneralProposalNodeDTO>> GenerateNodesAsync(
        GeneralProposalWorkflowCreateDTO dto,
        GeneralProposalWorkflow workflow
    )
    {
        // Get the last node ID (handle empty table)
        var lastId = await _context.GeneralProposalNodes.AnyAsync()
            ? await _context.GeneralProposalNodes.MaxAsync(x => x.Id)
            : 0;

        Employee employee =
            await _context.Employees.FirstOrDefaultAsync(e => e.Id == workflow.SenderId)
            ?? throw new InvalidOperationException($"Không tìm thấy nhân viên gửi trong hệ thống.");

        Employee approver =
            await _context.Employees.FirstOrDefaultAsync(e => e.Id == dto.ApproverId)
            ?? throw new InvalidOperationException(
                $"Không tìm thấy người phê duyệt trong hệ thống."
            );

        // Build up receiver IDs for workflow
        // Compose workflow steps

        // Initiate nodes creation: 2 nodes for now, first one has 2 participants, second one has 2 participants
        var steps = new List<(
            string Name,
            GeneralProposalStepType StepType,
            int Status,
            List<WorkflowNodeParticipant> nodeParticipants
        )>
        {
            (
                "Tạo tờ trình chung",
                GeneralProposalStepType.CreateForm,
                1,
                new List<WorkflowNodeParticipant> { }
            ),
            (
                "Người duyệt ký",
                GeneralProposalStepType.ExecutiveApproval,
                0,
                new List<WorkflowNodeParticipant> { }
            ),
        };

        // Build and add nodes
        var nodes = new List<GeneralProposalNode>();
        for (int i = 0; i < steps.Count; i++)
        {
            var step = steps[i];

            var node = new GeneralProposalNode
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
        _context.GeneralProposalNodes.AddRange(nodes);
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
                approver.MainId,
                nodes[1].Id,
                GeneralProposalStepType.ExecutiveApproval,
                0,
                null,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(48),
                false,
                ApprovalStatusType.PENDING,
                null
            ),
        };

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
                    WorkflowNodeType = "GeneralProposal",
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
            WorkflowNodeParticipants[1]
        };
        workflow.ParticipantIds = new List<int>
        {
            WorkflowNodeParticipants[0].EmployeeId,
            WorkflowNodeParticipants[1].EmployeeId,
        };

        // Create notification events for all participants
        var employeeIdsToNotify = new List<int> { employee.Id, approver.Id };
        employeeIdsToNotify = employeeIdsToNotify.Distinct().ToList();

        _logger.LogError("Employee IDs to notify: {Ids}", string.Join(", ", employeeIdsToNotify));

        var events = new List<CreateEvent>();
        employeeIdsToNotify.ForEach(id =>
        {
            events.Add(
                new CreateEvent
                {
                    WorkflowId = workflow.MainId.ToString(),
                    WorkflowType = "Tờ trình chung",
                    EmployeeId = id,
                    ApproverName = approver.GetDisplayName(),
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
                "Creating event for employee {EmployeeId}: {@Event}",
                @event.EmployeeId,
                @event
            );
            BackgroundJob.Enqueue<WorkflowEventHandler>(handler => handler.HandleCreation(@event));
        }

        await _context.SaveChangesAsync();

        return _mapper.Map<List<GeneralProposalNodeDTO>>(nodes);
    }

    public async Task<IEnumerable<GeneralProposalNodeDTO>> GetNodesByWorkflowIdAsync(int workflowId)
    {
        var nodes = await _context
            .GeneralProposalNodes
            .Where(n => n.WorkflowId == workflowId)
            .ToListAsync();

        var nodeIds = nodes.Select(n => n.Id).ToList();

        var participants = await _context
            .WorkflowNodeParticipants.Where(p => p.WorkflowNodeType == "GeneralProposal")
            .ToListAsync();

        foreach (var node in nodes)
        {
            node.WorkflowNodeParticipants = participants
                .Where(p => p.WorkflowNodeId == node.Id)
                .ToList();
        }

        var dtos = _mapper.Map<IEnumerable<GeneralProposalNodeDTO>>(nodes);

        _logger.LogInformation(
            "first node has {ParticipantCount} participants.",
            dtos.First().WorkflowNodeParticipants.Count()
        );
        return dtos;
    }

    public async Task<bool> FinalizeIfCompleteAsync(int workflowId)
    {
        GeneralProposalWorkflow workflow =
            await _dbSet
                .Include(wf => wf.GeneralProposalNodes)
                .FirstOrDefaultAsync(wf => wf.Id == workflowId)
            ?? throw new KeyNotFoundException($"Không tìm thấy tờ trình với id: {workflowId}.");

        if (workflow.IsDocumentGenerated)
        {
            throw new InvalidOperationException(
                "Quy trình đã có tài liệu đính kèm, không cần phải lấy bản vật lý lại."
            );
        }

        Employee employee =
            await _context
                .Employees.Include(e => e.CompanyInfo)
                .Include(e => e.Signature)
                .FirstOrDefaultAsync(e => e.Id == workflow.SenderId)
            ?? throw new InvalidOperationException($"Employee {workflow.SenderId} not found.");

        WorkflowNodeParticipant finalParticipant =
            _context
                .Set<WorkflowNodeParticipant>()
                .Where(p =>
                    p.WorkflowId == workflowId
                    && p.WorkflowNodeType == "GeneralProposal"
                    && p.ApprovalStatus == ApprovalStatusType.APPROVED
                )
                .OrderByDescending(p => p.Id)
                .FirstOrDefault()
            ?? throw new InvalidOperationException(
                "Quy trình chưa được phê duyệt hoàn tất, không thể lấy bản vật lý."
            );

        Employee approver =
            await _context
                .Employees.Include(e => e.CompanyInfo)
                .Include(e => e.Signature)
                .FirstOrDefaultAsync(e => e.Id == finalParticipant.EmployeeId)
            ?? throw new InvalidOperationException($"Không tìm ra người ký cuối trong hệ thống.");
        _logger.LogError(
            "Approver found: {ApproverName} (ID: {ApproverId})",
            approver.GetDisplayName(),
            approver.Id
        );
        var result = await GenerateGeneralProposalFinalDocument(employee, approver, workflow);

        return result;
    }

    public override async Task<GeneralProposalWorkflowDTO> CreateAsync(
    GeneralProposalWorkflowCreateDTO dto
)
{
    await using var transaction = await _context.Database.BeginTransactionAsync();
    try
    {
        // Map DTO to entity
        var entity = _mapper.Map<GeneralProposalWorkflow>(dto);

        // Save workflow first to get its Id
        _context.GeneralProposalWorkflows.Add(entity);
        await _context.SaveChangesAsync();

        entity.MainId = $"{DateTime.Now:yyyy}/{entity.Id}/TTC";
        entity.Name = $"Tờ trình chung số {entity.Id}, năm {DateTime.Now:yyyy}";

        // Generate workflow nodes
        var nodes = await GenerateNodesAsync(dto, entity);

        // 👉 Save lại sau khi cập nhật MainId + Name
        await _context.SaveChangesAsync();

        // 👉 Lưu DocumentAssociations nếu có
        if (entity.DocumentAssociations != null && entity.DocumentAssociations.Any())
        {
            foreach (var doc in entity.DocumentAssociations)
            {
                var newDocumentAssociation = new DocumentAssociation
                {
                    DocumentId = doc.Id,
                    EntityType = doc.TemplateKey,
                    EntityId = entity.Id,
                };
                _context.DocumentAssociations.Add(newDocumentAssociation);
            }

            await _context.SaveChangesAsync();
        }

        await transaction.CommitAsync();

        // Map to DTO sau khi commit
        var finalDto = _mapper.Map<GeneralProposalWorkflowDTO>(entity);
        finalDto.GeneralProposalNodes = _mapper.Map<List<GeneralProposalNodeDTO>>(nodes);
        return finalDto;
    }
    catch
    {
        await transaction.RollbackAsync();
        throw;
    }
}


    public override async Task<GeneralProposalWorkflowDTO?> GetByIdAsync(int id)
    {
        // Step 1: Get workflow and its nodes
        GeneralProposalWorkflow workflow =
            await _dbSet
                .Include(wf => wf.GeneralProposalNodes)
                .Include(wf => wf.Sender)
                .FirstOrDefaultAsync(wf => wf.Id == id)
            ?? throw new KeyNotFoundException($"Không tìm thấy tờ trình với id: {id}.");

        // Step 2: Load all participants in one query
        var nodeIds = workflow.GeneralProposalNodes.Select(n => n.Id).ToList();

        var participants = await _context
            .WorkflowNodeParticipants.Where(p =>
                nodeIds.Contains(p.WorkflowNodeId) && p.WorkflowNodeType == "GeneralProposal"
            )
            .Include(p => p.Employee)
            .ToListAsync();

        // Step 3: Assign participants to nodes
        foreach (var node in workflow.GeneralProposalNodes)
        {
            node.WorkflowNodeParticipants = participants
                .Where(p => p.WorkflowNodeId == node.Id)
                .ToList();
        }

        workflow.DocumentAssociations = await _context
            .DocumentAssociations.Where(da =>
                da.EntityId == workflow.Id && da.EntityType == "GeneralProposal"
            )
            .Include(da => da.Document) // Eager load the Document
            .Select(da => da.Document)
            .ToListAsync();

        var dto = _mapper.Map<GeneralProposalWorkflowDTO>(workflow);
        dto.SenderName = workflow.Sender.LastName + " " + workflow.Sender.MiddleName + " " + workflow.Sender.FirstName;
        dto.SenderMainId = workflow.Sender.MainId;
        return dto;
    }

    // Add document only at the end of the workflow. The newly created document will be attached to the workflow itself, not the nodes
    public async Task<bool> GenerateGeneralProposalFinalDocument(
        Employee employee,
        Employee approver,
        GeneralProposalWorkflow workflow
    )
    {
        string className = GetType().Name;
        string TemplateKey = className.Split(
            ["Node", "Service", "Workflow"],
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

        var Location = "2_Chung";
        var newFileName =
            $"{workflow.Id}-{today:yyyy-MM-dd}-{Location}-TTC-{employee.MainId}-v01{".docx"}";
        var newTargetPath = Path.Combine(
                "erp",
                "documents",
                Location,
                "Ho_So",
                "To_Trinh_Chung",
                newFileName
            )
            .Replace("\\", "/");

        // Filling in steps
        var placeholders = new Dictionary<string, (string Text, bool IsBold)>
        {
            // English
            ["about"] = (workflow.About ?? "", false),
            ["approverComment"] = (workflow.Comment ?? "", false),
            ["employeeName"] = (employee.GetDisplayName(), false),
            ["employeePosition"] = (employee.CompanyInfo?.Position ?? "", false),
            ["project"] = (workflow.ProjectName, false),
            ["description"] = (workflow.Description, false),
            ["reason"] = (workflow.Reason, false),
            ["proposal"] = (workflow.Proposal, false),

            // Sign Area
            ["employeeSignDate"] = (workflow.CreatedAt.ToString("dd/MM/yyyy"), false),
            ["employeeFullNameBottom"] = (employee.GetDisplayName(), false),

            ["approverPosition"] = (approver.CompanyInfo?.Position ?? "", true),
            ["approverName"] = (approver.GetDisplayName().ToUpperInvariant(), false),
            ["approverSignDate"] = (today.ToString("dd/MM/yyyy"), false),
            ["approverFullName"] = (approver.GetDisplayName().ToUpperInvariant(), false),
        };

        MemoryStream newMemoryDoc = await FileHandling.ToMemoryStreamAsync(newDoc);

        WordBookmarkReplacer.ReplacePlaceholders(newMemoryDoc, placeholders);

        newMemoryDoc = await WordImageInserter.InsertImageAtBookmarkAsync(
            newMemoryDoc,
            "employeeSign",
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
            Description = $"Mẫu tờ trình phép của {employee.GetDisplayName()}",
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
