using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using portal.Enums;

namespace portal.Models;

public abstract class BaseWorkflowNode : BaseModel
{
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string Description { get; set; } = null!;
    [Required]
    public GeneralWorkflowStatusType Status { get; set; }

    [Required]
    [ForeignKey("Workflow")]
    public int WorkflowId { get; set; }

    // Reference to the employee who has a RACI role in the workflow. By standard, every node must need at least 1 employee
    public virtual List<WorkflowNodeParticipant> WorkflowParticipants { get; set; } = new List<WorkflowNodeParticipant>();

    // Many to many to docs
    public List<DocumentAssociation> DocumentAssociations { get; set; } = new List<DocumentAssociation>();
}