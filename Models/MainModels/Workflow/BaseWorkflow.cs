using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using portal.Enums;

namespace portal.Models;

public abstract class BaseWorkflow : BaseModel
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public int SenderId { get; set; }
    public Employee Sender { get; set; } = null!;

    [Required]
    public string Description { get; set; } = string.Empty;
    public string? RejectReason { get; set; }

    // Status of the workflow, e.g., Draft, Active, Archived
    public GeneralWorkflowStatusType Status { get; set; }

    [NotMapped]
    public List<WorkflowNodeParticipant> WorkflowNodeParticipants { get; set; } =
        new List<WorkflowNodeParticipant>();

    // Reference to document system  (retrieved from nodes with LINQ)

    [NotMapped]
    public List<Document> DocumentAssociations { get; set; } = new List<Document>();
}
