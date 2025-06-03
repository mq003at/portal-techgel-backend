using System.ComponentModel.DataAnnotations.Schema;
using portal.Enums;

namespace portal.Models;

public abstract class BaseWorkflow : BaseModel
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    // Status of the workflow, e.g., Draft, Active, Archived
    public GeneralWorkflowStatusType Status { get; set; } = GeneralWorkflowStatusType.Draft;

    // Who can see the workflow?
    public ICollection<int> ReceiverIds { get; set; } = new List<int>();
    [NotMapped]
    public ICollection<string> ReceiverNames { get; set; } = new List<string>();


    // Metadata about who created or approved the workflow

    public ICollection<int> DraftedByIds { get; set; } = new List<int>();
    [NotMapped]
    public ICollection<string> DraftedByNames { get; set; } = new List<string>();

    public ICollection<int> HasBeenApprovedByIds { get; set; } = new List<int>();

    [NotMapped]
    public ICollection<string> HasBeenApprovedByNames { get; set; } = new List<string>();

    public ICollection<DateTime> ApprovedDates { get; set; } = new List<DateTime>();
}