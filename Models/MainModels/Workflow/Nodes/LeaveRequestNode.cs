using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace portal.Models;

public class LeaveRequestNode : BaseWorkflowNode
{
    // Indicates the step in this workflow (used with participants' steps to perform services)
    public LeaveApprovalStepType StepType { get; set; }
    [Required]
    public int WorkflowId { get; set; }
    [ForeignKey("WorkflowId")]
    public virtual LeaveRequestWorkflow Workflow { get; set; } = null!;

}