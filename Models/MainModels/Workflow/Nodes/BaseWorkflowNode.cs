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

    // null = no approval logic, workflownode participants will only have 1 element in the  list
    public int WorkflowId { get; set; }

    [ForeignKey("WorkflowId")]
    // Not map field so that we do not cluster up the db
    [NotMapped]
    public List<WorkflowNodeParticipant> WorkflowNodeParticipants { get; set; } =
        new List<WorkflowNodeParticipant>();
}
