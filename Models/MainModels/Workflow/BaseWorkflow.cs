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
    public List<int> ReceiverIds { get; set; } = new List<int>();
    [NotMapped]
    public List<string> ReceiverNames { get; set; } = new List<string>();


    // Metadata about who created or approved the workflow

    public List<int> DraftedByIds { get; set; } = new List<int>();
    [NotMapped]
    public List<string> DraftedByNames { get; set; } = new List<string>();

    public List<int> HasBeenApprovedByIds { get; set; } = new List<int>();

    [NotMapped]
    public List<string> HasBeenApprovedByNames { get; set; } = new List<string>();

    public int SenderId { get; set; }
    [NotMapped]
    public string SenderName { get; set; } = string.Empty;
    [NotMapped]
    public Employee? Sender { get; set; } = null;
    public List<DateTime> ApprovedDates { get; set; } = new List<DateTime>();
}