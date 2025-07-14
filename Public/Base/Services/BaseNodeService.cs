using System.Text.Json;
using AutoMapper;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using portal.Db;
using portal.DTOs;
using portal.Enums;
using portal.Extensions;
using portal.Models;

namespace portal.Services;

public abstract class BaseNodeService<TModel, TReadDTO, TCreateDTO, TUpdateDTO, TWorkflowModel>
    : BaseService<TModel, TReadDTO, TCreateDTO, TUpdateDTO>,
        IBaseNodeService<TModel, TReadDTO, TCreateDTO, TUpdateDTO, TWorkflowModel>
    where TModel : BaseWorkflowNode, new()
    where TReadDTO : WorkflowNodeDTO
    where TCreateDTO : WorkflowNodeCreateDTO
    where TUpdateDTO : WorkflowNodeUpdateDTO
    where TWorkflowModel : BaseWorkflow
{
    protected new readonly ApplicationDbContext _context;
    protected new readonly IMapper _mapper;
    protected new readonly ILogger<
        BaseNodeService<TModel, TReadDTO, TCreateDTO, TUpdateDTO, TWorkflowModel>
    > _logger;
    protected readonly ICapPublisher _capPublisher;

    public BaseNodeService(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<BaseNodeService<TModel, TReadDTO, TCreateDTO, TUpdateDTO, TWorkflowModel>> logger,
        ICapPublisher capPublisher
    )
        : base(context, mapper, logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
        _capPublisher = capPublisher;
    }

    public async Task<bool> ApproveAsync(int nodeId, ApproveWithCommentDTO dto)
    {
        int approverId = dto.ApproverId;

        // Fetch the node and its workflow
        TModel node =
            await _context.Set<TModel>().FirstOrDefaultAsync(n => n.Id == nodeId)
            ?? throw new InvalidOperationException(
                "Không tìm thấy bước trong quy trình này. Lỗi hệ thống!"
            );

        TWorkflowModel workflow =
            await _context.Set<TWorkflowModel>().FirstOrDefaultAsync(wf => wf.Id == node.WorkflowId)
            ?? throw new InvalidOperationException("Không tìm thấy quy trình.");

        // Get the Template Key
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

        // Fetch participants based on Template
        List<WorkflowNodeParticipant> participants = _context
            .Set<WorkflowNodeParticipant>()
            .Where(p => p.WorkflowNodeId == node.Id && p.WorkflowNodeType == TemplateKey)
            .Include(p => p.Employee)
            .ToList();

        _logger.LogError(
            "Found {ParticipantCount} participants for node {NodeId} in workflow {WorkflowId}",
            participants.Count,
            node.Id,
            workflow.Id
        );

        WorkflowNodeParticipant participant =
            participants.FirstOrDefault(p => p.EmployeeId == approverId)
            ?? throw new InvalidOperationException("Không tìm thấy người phê duyệt cho bước này.");

        if (approverId != participant.EmployeeId)
            throw new InvalidOperationException("Bạn không được phép phê duyệt bước này.");

        if (participant.ApprovalStatus != ApprovalStatusType.PENDING)
            throw new InvalidOperationException("Bạn chỉ có thể phê duyệt bước đang chờ.");

        if (participant.ApprovalStartDate > DateTime.UtcNow)
            throw new InvalidOperationException(
                "Bạn không thể phê duyệt bước này trước ngày bắt đầu phê duyệt."
            );

        if (participant.RaciRole == WorkflowParticipantRoleType.INFORMED)
            throw new InvalidOperationException(
                "Bạn không có quyền phê duyệt bước này vì bạn chỉ là người được thông báo."
            );

        // If approving is successful, update the wf to pending so that it cannot be changed or delete anymore
        workflow.Status = GeneralWorkflowStatusType.PENDING;

        // Uncomment if you want to enforce an approval deadline
        // if (participant.ApprovalDeadline < DateTime.UtcNow)
        // {
        // return "You cannot approve this node after the approval deadline.";
        // }

        participant.ApprovalStatus = ApprovalStatusType.APPROVED;
        participant.ApprovalDate = DateTime.UtcNow;

        if (participant.ApprovalDate > participant.ApprovalStartDate)
            // Calculate Turnaround Time (TAT) as the difference between ApprovalDate and ApprovalStartDate
            participant.TAT = participant.ApprovalDate - participant.ApprovalStartDate;
        else
            participant.TAT = TimeSpan.Zero;

        bool allApproved = false;
        if (node.NodeApprovalLogic == WorkflowNodeApprovalLogic.ANY_ONE)
            allApproved = participants.Any(p => p.ApprovalStatus == ApprovalStatusType.APPROVED);
        if (node.NodeApprovalLogic == WorkflowNodeApprovalLogic.EVERYONE)
            allApproved = participants.All(p => p.ApprovalStatus == ApprovalStatusType.APPROVED);

        // If approval condition satisified, proceed node, or end the workflow
        if (allApproved)
        {
            node.Status = GeneralWorkflowStatusType.APPROVED;

            // Check if this is the final node in the workflow
            var isFinalNode = !_context
                .Set<TModel>()
                .Any(n =>
                    n.WorkflowId == node.WorkflowId
                    && n.Id != node.Id
                    && n.Status != GeneralWorkflowStatusType.APPROVED
                );

            if (isFinalNode)
            {
                workflow.Status = GeneralWorkflowStatusType.APPROVED;
                if (dto.Comment != null)
                {
                    workflow.Comment = dto.Comment;
                }
                // await _workflowService.FinalizeIfCompleteAsync(workflow, approverId, nodeId);
            }
        }

        string vietnameseDisplayName = WorkflowResolver.GetDisplayName(TemplateKey);
        var @event = new ApprovalEvent
        {
            WorkflowId = workflow.MainId.ToString(),
            WorkflowType = vietnameseDisplayName,
            EmployeeId = workflow.SenderId,
            ApproverName = participant.Employee.GetDisplayName(),
            ApprovedAt = DateTime.UtcNow,
            TriggeredBy = participant.Employee.MainId.ToString(),
        };

        await _capPublisher.PublishAsync("workflow.approved", @event);

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RejectAsync(int nodeId, RejectDTO dto)
    {
        // Get the Template Key
        string className = GetType().Name;
        string TemplateKey = className.Split(
            ["Node", "Service", "Workflow"],
            StringSplitOptions.None
        )[0];

        TModel node =
            await _context.Set<TModel>().FirstOrDefaultAsync(n => n.Id == nodeId)
            ?? throw new InvalidOperationException("Không tìm thấy bước quy trình.");

        TWorkflowModel workflow =
            await _context.Set<TWorkflowModel>().FirstOrDefaultAsync(wf => wf.Id == node.WorkflowId)
            ?? throw new InvalidOperationException("Không tìm thấy quy trình.");

        List<WorkflowNodeParticipant> participants = _context
            .Set<WorkflowNodeParticipant>()
            .Where(p => p.WorkflowNodeId == node.Id && p.WorkflowNodeType == TemplateKey)
            .Include(p => p.Employee)
            .ToList();

        WorkflowNodeParticipant participant =
            participants.FirstOrDefault(p => p.EmployeeId == dto.ApproverId)
            ?? throw new InvalidOperationException("Không tìm thấy người tham gia.");

        _logger.LogError(
            "Rejecting node {NodeId} in workflow {WorkflowId} by approver {ApproverId}",
            node.Id,
            workflow.Id,
            JsonSerializer.Serialize(participants)
        );

        if (dto.ApproverId != participant.EmployeeId)
            throw new InvalidOperationException("Bạn không có quyền từ chối bước này.");

        if (participant == null)
            throw new InvalidOperationException("Bạn không có quyền từ chối bước này.");

        if (participant.ApprovalStatus != ApprovalStatusType.PENDING)
            throw new InvalidOperationException("Bạn chỉ có thể từ chối bước đang chờ phê duyệt.");

        if (participant.ApprovalStartDate > DateTime.UtcNow)
            throw new InvalidOperationException(
                "Bạn không thể từ chối bước này trước thời gian bắt đầu phê duyệt."
            );

        _logger.LogError(
            "Generating final document for workflow {WorkflowId} with template key {TemplateKey}",
            workflow.Id,
            TemplateKey
        );

        // Update participant status
        participant.ApprovalStatus = ApprovalStatusType.REJECTED;
        participant.ApprovalDate = DateTime.UtcNow;
        if (participant.ApprovalDate > participant.ApprovalStartDate)
            // Calculate Turnaround Time (TAT) as the difference between ApprovalDate and ApprovalStartDate
            participant.TAT = participant.ApprovalDate - participant.ApprovalStartDate;
        else
            participant.TAT = TimeSpan.Zero;

        // Mark node as Rejected immediately
        node.Status = GeneralWorkflowStatusType.REJECTED;

        // Also mark the entire workflow as Rejected

        workflow.Status = GeneralWorkflowStatusType.REJECTED;
        workflow.RejectReason = dto.RejectReason;

        string vietnameseDisplayName = WorkflowResolver.GetDisplayName(TemplateKey);
        var @event = new RejectEvent
        {
            WorkflowId = workflow.MainId.ToString(),
            WorkflowType = vietnameseDisplayName,
            EmployeeId = workflow.SenderId,
            ApproverName = participant.Employee.GetDisplayName(),
            RejectedAt = DateTime.UtcNow,
            TriggeredBy = participant.Employee.MainId.ToString(),
            Reason = dto.RejectReason,
        };

        await _capPublisher.PublishAsync("workflow.rejected", @event);

        await _context.SaveChangesAsync();

        return true;
    }
}
