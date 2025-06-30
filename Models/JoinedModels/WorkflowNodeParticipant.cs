using System.ComponentModel.DataAnnotations;
using DocumentFormat.OpenXml.Math;
using portal.Enums;

namespace portal.Models;

// It will be in the nodes, and will be referenced in the workflow to gather a list of participants
public class WorkflowNodeParticipant : BaseModelWithOnlyId
{
    [Required]
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    [Required]
    // Shallow map cuz it connects to parent node, not derived.
    public int WorkflowNodeId { get; set; }
    public string WorkflowNodeType { get; set; } = null!;

    // Order in the nodes
    public int Order { get; set; }

    // [Required]
    // public required WorkflowRole WorkflowRole { get; set; }

    // Workflow's step in a node (step 1,2,3,4 depending on what is defined so that one person can approve in many different steps)
    public int WorkflowNodeStepType { get; set; }

    // Approval related properties -> start date, end date, time approved, approved, or rejected, time to approve
    public DateTime? ApprovalStartDate { get; set; }
    public DateTime? ApprovalDate { get; set; }
    public DateTime? ApprovalDeadline { get; set; }
    public ApprovalStatusType ApprovalStatus { get; set; }
    public TimeSpan? TAT { get; set; } = TimeSpan.Zero;
}
