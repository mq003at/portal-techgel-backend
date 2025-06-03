using portal.Models;

namespace portal.DTOs;

public class LeaveRequestNodeDTO : WorkflowNodeDTO<LeaveRequestNode>
{
    public int LeaveRequestWorkflowId { get; set; }
    public string LeaveRequestName { get; set; } = string.Empty;

    public LeaveApprovalStepType StepType { get; set; }
}

public class CreateLeaveRequestNodeDTO : CreateWorkflowNodeDTO<LeaveRequestNode>
{
    public int LeaveRequestWorkflowId { get; set; }
    public LeaveApprovalStepType StepType { get; set; }
}
public class UpdateLeaveRequestNodeDTO : UpdateWorkflowNodeDTO<LeaveRequestNode>
{
    public int? LeaveRequestWorkflowId { get; set; }
    public LeaveApprovalStepType? StepType { get; set; }
}