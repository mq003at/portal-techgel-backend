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
    CreateLeaveRequestNodeDTO,
    UpdateLeaveRequestNodeDTO>,
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

    public async Task<bool> ApproveAsync(int nodeId, int approverId, string? comment = null)
    {
        var node = await _context.LeaveRequestNodes.FindAsync(nodeId);
        if (node == null) return false;

        // Ensure array never null (đề phòng DB đang null)
        node.ApprovedByIds ??= new List<int>();
        node.HasBeenApprovedByIds ??= new List<int>();

        // Block neu node status khong phai Pending
        if (node.Status != GeneralWorkflowStatusType.Pending)
            throw new InvalidOperationException(
                $"Node {nodeId} is not in a pending state and cannot be approved."
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

        var workflow = await _context.LeaveRequestWorkflows
            .FindAsync(node.LeaveRequestWorkflowId);

        if (workflow is null)
        {
            throw new InvalidOperationException(
                $"Workflow with ID {node.LeaveRequestWorkflowId} not found."
            );
        }
        // Thêm vào danh sách đã approve
        node.HasBeenApprovedByIds.Add(approverId);
        workflow.HasBeenApprovedByIds.Add(approverId);

        node.ApprovedDates ??= new List<DateTime>();
        node.ApprovedDates.Add(DateTime.UtcNow);
        workflow.ApprovedDates.Add(DateTime.UtcNow);

        // Check đã đủ approved hay chưa
        var hasNodeAllApproved = Helpers.ArrayHelper.AreArraysEqual(
            node.ApprovedByIds, node.HasBeenApprovedByIds
        );

        if (hasNodeAllApproved)
        {
            node.Status = GeneralWorkflowStatusType.Approved;

            // Neu la node 3 (final node), update the workflow status
            if (node.StepType == LeaveApprovalStepType.ExecutiveApproval)
            {
                workflow.Status = GeneralWorkflowStatusType.Approved;
                _context.LeaveRequestWorkflows.Update(workflow);

                var employee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.Id == workflow.EmployeeId);
                if (employee is null)
                {
                    throw new InvalidOperationException(
                        $"Employee with ID {workflow.EmployeeId} not found."
                    );
                }

                employee.CompanyInfo.AnnualLeaveTotalDays = workflow.FinalEmployeeAnnualLeaveTotalDays;
                _context.Employees.Update(employee);
            }
            else
            {
                // Neu khong phai final node, update the next node status to Pending
                var nextNode = await _context.LeaveRequestNodes
                    .FirstOrDefaultAsync(n => n.Id == nodeId + 1);
                if (nextNode != null)
                {
                    nextNode.Status = GeneralWorkflowStatusType.Pending;
                    _context.LeaveRequestNodes.Update(nextNode);
                }
            }
            _logger.LogInformation(
                "Node {NodeId} has been fully approved by all required approvers.",
                nodeId
            );
        }

        _context.LeaveRequestNodes.Update(node);
        await _context.SaveChangesAsync();
        return true;
    }

}