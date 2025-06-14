using portal.Models;

namespace portal.DTOs;

public class LeaveRequestNodeDTO : WorkflowNodeDTO
{
    public LeaveApprovalStepType StepType { get; set; }
    public string StepTypeName { get; set; } = string.Empty;
}

public class LeaveRequestNodeCreateDTO : WorkflowNodeCreateDTO
{
    public LeaveApprovalStepType StepType { get; set; }
}

public class LeaveRequestNodeUpdateDTO : WorkflowNodeUpdateDTO
{
    public LeaveApprovalStepType StepType { get; set; }
}