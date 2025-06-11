using System.ComponentModel.DataAnnotations;
using DocumentFormat.OpenXml.Math;
using portal.Enums;

namespace portal.Models;

// It will be in the nodes, and will be referenced in the workflow to gather a list of participants
public class WorkflowNodeParticipant : BaseModelWithOnlyId
{
    [Required]
    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }
    [Required]
    public int WorkflowNodeId { get; set; }
    public BaseWorkflowNode? BaseWorkflowNode { get; set; }
    // RACIQ in the current workflow
    public WorkflowParticipantRoleType RaciRole { get; set; }

    // Specific role in the workflow (are they the Sender, Leaver, etc.)
    [Required]
    public required string WorkflowRole { get; set; }

    // Workflow's step in a node (step 1,2,3,4 depending on what is defined so that one person can approve in many different steps)
    public int WorkflowNodeStepType { get; set; }
    // Approval related properties
    public DateTime? ApprovalDate { get; set; }
    public DateTime? ApprovalDeadline { get; set; }
    public bool? IsApproved { get; set; }
    public bool? IsRejected { get; set; }
    public TimeSpan TAT { get; set; } = TimeSpan.Zero;
}