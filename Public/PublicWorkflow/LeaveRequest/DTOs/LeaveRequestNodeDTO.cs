using portal.Models;

namespace portal.DTOs;

public class LeaveRequestNodeDTO : WorkflowNodeDTO
{
    public new LeaveApprovalStepType StepType { get; set; }
    public string StepTypeName { get; set; } = null!;
}

public class LeaveRequestNodeCreateDTO : WorkflowNodeCreateDTO
{
    public LeaveApprovalStepType StepType { get; set; }
}

public class LeaveRequestNodeUpdateDTO : WorkflowNodeUpdateDTO
{
    public LeaveApprovalStepType StepType { get; set; }
}