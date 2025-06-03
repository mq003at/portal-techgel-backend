using System.ComponentModel.DataAnnotations.Schema;
namespace portal.Models;

public class LeaveRequestNode : WorkflowNode
{
    public int LeaveRequestWorkflowId { get; set; }

    [NotMapped]
    public LeaveRequestWorkflow LeaveRequestWorkflow { get; set; } = new();

    public LeaveApprovalStepType StepType { get; set; }
}