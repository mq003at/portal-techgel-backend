using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using portal.Enums;

namespace portal.Models;

public abstract class BaseWorkflow : BaseModel
{
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Description { get; set; } = string.Empty;

    // Status of the workflow, e.g., Draft, Active, Archived
    public GeneralWorkflowStatusType Status { get; set; } = GeneralWorkflowStatusType.Draft;

    // Participants reference (retrieved from nodes with LINQ) 
    public List<WorkflowNodeParticipant> WorkflowParticipants { get; set; } = new List<WorkflowNodeParticipant>();
    // Reference to document system  (retrieved from nodes with LINQ)
    public List<DocumentAssociation> DocumentAssociations { get; set; } = new List<DocumentAssociation>();
}