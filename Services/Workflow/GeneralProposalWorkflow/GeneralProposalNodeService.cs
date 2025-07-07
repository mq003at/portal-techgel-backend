namespace portal.Services;

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using portal.Db;
using portal.DTOs;
using portal.Enums;
using portal.Models;

public class GeneralProposalNodeService
    : BaseService<
        GeneralProposalNode,
        GeneralProposalNodeDTO,
        GeneralProposalNodeCreateDTO,
        GeneralProposalNodeUpdateDTO
    >,
        IGeneralProposalNodeService
{
    private new readonly ApplicationDbContext _context;
    private new readonly IMapper _mapper;
    private new readonly ILogger<GeneralProposalNodeService> _logger;
    private readonly IGeneralProposalWorkflowService _workflowService;

    public GeneralProposalNodeService(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<GeneralProposalNodeService> logger,
        IGeneralProposalWorkflowService workflowService
    )
        : base(context, mapper, logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
        _workflowService = workflowService;
    }

    public async Task<string> ApproveAsync(int nodeId, ApproveWithCommentDTO dto)
    {
        int approverId = dto.ApproverId;
        string comment = dto.Comment;

        // Fetch the node and its workflow

        GeneralProposalNode node =
            await _context
                .GeneralProposalNodes.Include(n => n.Workflow)
                .FirstOrDefaultAsync(n => n.Id == nodeId)
            ?? throw new InvalidOperationException(
                "Không tìm thấy bước trong quy trình này. Lỗi hệ thống!"
            );

        GeneralProposalWorkflow workflow =
            node.Workflow
            ?? throw new InvalidOperationException("Không tìm thấy quy trình cho bước này.");

        // Manually fetch participants and attach to node
        var participants = await _context
            .WorkflowNodeParticipants.Where(p =>
                p.WorkflowNodeId == nodeId && p.WorkflowNodeType == "GeneralProposal"
            )
            .ToListAsync();

        _logger.LogInformation("P0: {p0}", participants.Count);
        WorkflowNodeParticipant participant =
            participants.FirstOrDefault(p => p.EmployeeId == approverId)
            ?? throw new InvalidOperationException("Không tìm thấy người phê duyệt cho bước này.");

        // Check if approverId = participant.EmployeeId
        if (approverId != participant.EmployeeId)
            return "Bạn không được phép phê duyệt bước này.";

        if (participant.ApprovalStatus != ApprovalStatusType.PENDING)
            return "Bạn chỉ có thể phê duyệt bước đang chờ.";

        if (participant.ApprovalStartDate > DateTime.UtcNow)
            return "Bạn không thể phê duyệt bước này trước ngày bắt đầu phê duyệt.";

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

        // If all participants approved
        var allApproved = participants
            .Where(p => p.WorkflowNodeStepType == 1)
            .Any(p => p.ApprovalStatus == ApprovalStatusType.APPROVED);
        if (allApproved)
        {
            node.Status = GeneralWorkflowStatusType.APPROVED;

            // Check if this is the final node in the workflow
            var isFinalNode = !_context.LeaveRequestNodes.Any(n =>
                n.WorkflowId == node.WorkflowId
                && n.Id != node.Id
                && n.Status != GeneralWorkflowStatusType.APPROVED
            );

            if (isFinalNode)
            {
                workflow.Status = GeneralWorkflowStatusType.APPROVED;
                workflow.Comment = comment;
                await _workflowService.FinalizeIfCompleteAsync(workflow, approverId, nodeId);
            }
        }

        await _context.SaveChangesAsync();
        return "Ký duyệt thành công.";
    }

    public async Task<string> RejectAsync(int nodeId, RejectDTO dto)
    {
        LeaveRequestNode node =
            await _context
                .LeaveRequestNodes.Include(n => n.Workflow)
                .FirstOrDefaultAsync(n => n.Id == nodeId)
            ?? throw new InvalidOperationException("Không tìm thấy bước quy trình.");

        LeaveRequestWorkflow workflow =
            node.Workflow
            ?? throw new InvalidOperationException("Không tìm thấy quy trình cho bước này.");

        List<WorkflowNodeParticipant> participants =
            node.WorkflowNodeParticipants
            ?? throw new InvalidOperationException("Bước này không có người tham gia.");
        WorkflowNodeParticipant participant =
            participants.FirstOrDefault(p => p.EmployeeId == dto.ApproverId)
            ?? throw new InvalidOperationException("Không tìm thấy người tham gia.");

        if (dto.ApproverId != participant.EmployeeId)
            return "Bạn không có quyền từ chối bước này.";

        if (participant == null)
            return "Bạn không có quyền từ chối bước này.";

        if (participant.ApprovalStatus != ApprovalStatusType.PENDING)
            return "Bạn chỉ có thể từ chối bước đang chờ phê duyệt.";

        if (participant.ApprovalStartDate > DateTime.UtcNow)
            return "Bạn không thể từ chối bước này trước thời gian bắt đầu phê duyệt.";

        // Update participant status
        participant.ApprovalStatus = ApprovalStatusType.REJECTED;
        participant.ApprovalDate = DateTime.UtcNow;

        // Mark node as Rejected immediately
        node.Status = GeneralWorkflowStatusType.REJECTED;

        // Also mark the entire workflow as Rejected
        if (workflow != null)
        {
            workflow.Status = GeneralWorkflowStatusType.REJECTED;
            workflow.RejectReason = dto.RejectReason;
        }

        await _context.SaveChangesAsync();

        return "Từ chối thành công.";
    }
}
