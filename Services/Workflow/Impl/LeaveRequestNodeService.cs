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

    public LeaveRequestNodeService(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<LeaveRequestNodeService> logger
    ) : base(context, mapper, logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<string> ApproveAsync(int nodeId, int approverId)
{
    var node = await _context.LeaveRequestNodes
        .Include(n => n.WorkflowParticipants)
        .FirstOrDefaultAsync(n => n.Id == nodeId);

    if (node == null)
        return "Node not found.";

    var participant = node.WorkflowParticipants
        .FirstOrDefault(p => p.EmployeeId == approverId);

    if (participant == null)
        return "You are not authorized to approve this node.";

    if (participant.HasApproved == true)
        return "You have already approved this node.";

    participant.HasApproved = true;
    participant.ApprovalDate = DateTime.UtcNow;

    // If all participants approved
    var allApproved = node.WorkflowParticipants.All(p => p.HasApproved == true);
    if (allApproved)
    {
        node.Status = GeneralWorkflowStatusType.Approved;

        // Attach signature if applicable
        // var documentIds = node.DocumentAssociations.Select(d => d.DocumentId).ToList();
        // foreach (var docId in documentIds)
        // {
        //     _context.DocumentSignatures.Add(new DocumentSignature
        //     {
        //         DocumentId = docId,
        //         EmployeeId = approverId,
        //         SignedAt = DateTime.UtcNow,
        //     });
        // }

        // Check if this is the final node in the workflow
        var isFinalNode = !_context.LeaveRequestNodes
            .Any(n => n.WorkflowId == node.WorkflowId && n.Id != node.Id && n.Status != GeneralWorkflowStatusType.Approved);

        if (isFinalNode)
        {
            var workflow = _context.LeaveRequestWorkflows
                .FirstOrDefault(w => w.Id == node.WorkflowId);
            if (workflow != null)
            {
                workflow.Status = GeneralWorkflowStatusType.Approved;
            }
        }
    }

    await _context.SaveChangesAsync();
    return "Approval recorded successfully.";
}

    public async Task<string> RejectAsync(int nodeId, int approverId)
{
    var node = await _context.LeaveRequestNodes
        .Include(n => n.WorkflowParticipants)
        .FirstOrDefaultAsync(n => n.Id == nodeId);

    if (node == null)
        return "Node not found.";

    var participant = node.WorkflowParticipants
        .FirstOrDefault(p => p.EmployeeId == approverId);

    if (participant == null)
        return "You are not allowed to reject this node.";

    if (participant.HasRejected == true)
        return "You have already rejected.";

    if (participant.HasApproved == true)
        return "You already approved this node. Cannot reject.";

    // Update participant status
    participant.HasRejected = true;
    participant.ApprovalDate = DateTime.UtcNow;

    // Mark node as Rejected immediately
    node.Status = GeneralWorkflowStatusType.Rejected;

    // Also mark the entire workflow as Rejected
    var workflow = _context.LeaveRequestWorkflows
        .FirstOrDefault(w => w.Id == node.WorkflowId);
    if (workflow != null)
    {
        workflow.Status = GeneralWorkflowStatusType.Rejected;
    }

    await _context.SaveChangesAsync();

    return "Node rejected successfully.";
}


}