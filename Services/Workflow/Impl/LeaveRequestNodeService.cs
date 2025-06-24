namespace portal.Services;

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using portal.Db;
using portal.DTOs;
using portal.Enums;
using portal.Models;

public class LeaveRequestNodeService : BaseService<
    LeaveRequestNode,
    LeaveRequestNodeDTO,
    LeaveRequestNodeCreateDTO,
    LeaveRequestNodeUpdateDTO>,
    ILeaveRequestNodeService
{
    private new readonly ApplicationDbContext _context;
    private new readonly IMapper _mapper;
    private new readonly ILogger<LeaveRequestNodeService> _logger;
    private readonly ILeaveRequestWorkflowService _workflowService;

    public LeaveRequestNodeService(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<LeaveRequestNodeService> logger,
        ILeaveRequestWorkflowService workflowService
    ) : base(context, mapper, logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
        _workflowService = workflowService;
    }
    
    public async Task<string> ApproveAsync(int nodeId, int approverId)
    {
        LeaveRequestNode node = await _context.LeaveRequestNodes
            .FirstOrDefaultAsync(n => n.Id == nodeId) ?? throw new InvalidOperationException("Node not found.");

        // Manually fetch participants and attach to node
        var participants = await _context.WorkflowNodeParticipants
            .Where(p => p.WorkflowNodeId == nodeId && p.WorkflowNodeType == "LeaveRequest")
            .ToListAsync();
        

        _logger.LogInformation("P0: {p0}", participants.Count);
        WorkflowNodeParticipant participant = participants
            .FirstOrDefault(p => p.EmployeeId == approverId) ?? throw new InvalidOperationException("You are not authorized to approve this node.");

        // Check if approverId = participant.EmployeeId
        if (approverId != participant.EmployeeId)
            return "You are not allowed to approve this node."; 

        if (participant.ApprovalStatus != ApprovalStatusType.PENDING)
            return "You can only approve the PENDING node.";

        if (participant.ApprovalStartDate > DateTime.UtcNow)
            return "You cannot approve this node before the approval start date.";

        // Uncomment if you want to enforce an approval deadline    
        // if (participant.ApprovalDeadline < DateTime.UtcNow)
        // {
        // return "You cannot approve this node after the approval deadline.";
        // }

        participant.ApprovalStatus = ApprovalStatusType.APPROVED;
        participant.ApprovalDate = DateTime.UtcNow;
        participant.TAT = participant.ApprovalDate - participant.ApprovalStartDate;

        // If all participants approved
        var allApproved = participants
            .Where(p => p.WorkflowNodeStepType == 1)
            .Any(p => p.ApprovalStatus == ApprovalStatusType.APPROVED);
        if (allApproved)
        {
            node.Status = GeneralWorkflowStatusType.APPROVED;

            // Check if this is the final node in the workflow
            var isFinalNode = !_context.LeaveRequestNodes
                .Any(n => n.WorkflowId == node.WorkflowId && n.Id != node.Id && n.Status != GeneralWorkflowStatusType.APPROVED);

            if (isFinalNode)
            {
                LeaveRequestWorkflow workflow = _context.LeaveRequestWorkflows
                    .FirstOrDefault(w => w.Id == node.WorkflowId) ?? throw new InvalidOperationException("Workflow not found.");
                _logger.LogInformation("Final node reached for workflow ID: {WorkflowId} with {Name}", workflow.Id, workflow.Name);
                if (workflow != null)
                {
                    workflow.Status = GeneralWorkflowStatusType.APPROVED;
                    await _workflowService.FinalizeIfCompleteAsync(workflow, approverId);
                }
            }
        }

        await _context.SaveChangesAsync();
        return "Approval recorded successfully.";
    }

    public async Task<string> RejectAsync(int nodeId, int approverId, string RejectReason)
    {
        LeaveRequestNode node = await _context.LeaveRequestNodes
            .Include(n => n.WorkflowNodeParticipants )
            .FirstOrDefaultAsync(n => n.Id == nodeId) ?? throw new InvalidOperationException("Node not found.");

        List<WorkflowNodeParticipant> participants = node.WorkflowNodeParticipants ?? throw new InvalidOperationException("Node has no participants.");
        WorkflowNodeParticipant participant = participants
            .FirstOrDefault(p => p.EmployeeId == approverId) ?? throw new InvalidOperationException("You are not authorized to approve this node.");

        if (approverId != participant.EmployeeId)
            return "You are not allowed to reject this node."; 

        if (participant == null)
            return "You are not allowed to reject this node.";

        if (participant.ApprovalStatus != ApprovalStatusType.PENDING)
            return "You can only reject the PENDING node.";

        if (participant.ApprovalStartDate > DateTime.UtcNow)
            return "You cannot reject this node before the approval start date.";

        // Update participant status
        participant.ApprovalStatus = ApprovalStatusType.REJECTED;
        participant.ApprovalDate = DateTime.UtcNow;

        // Mark node as Rejected immediately
        node.Status = GeneralWorkflowStatusType.REJECTED;

        // Also mark the entire workflow as Rejected
        var workflow = _context.LeaveRequestWorkflows
            .FirstOrDefault(w => w.Id == node.WorkflowId);
            if (workflow != null)
            {
                workflow.Status = GeneralWorkflowStatusType.REJECTED;
                workflow.RejectReason = RejectReason;
            }

        await _context.SaveChangesAsync();

        return "Node rejected successfully.";
    }


}