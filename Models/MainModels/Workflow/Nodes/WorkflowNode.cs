using System.ComponentModel.DataAnnotations.Schema;
using portal.Enums;

namespace portal.Models;

abstract public class WorkflowNode : BaseModel
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public GeneralWorkflowStatusType Status { get; set; }

    public int SenderId { get; set; }
    [NotMapped]
    public string SenderName { get; set; } = string.Empty;
    [NotMapped]
    public Employee Sender { get; set; } = null!;
    public List<int>? ApprovedByIds { get; set; } = new List<int>();

    [NotMapped]
    public List<string>? ApprovedByNames { get; set; } = new List<string>();

    public List<int>? HasBeenApprovedByIds { get; set; } = new List<int>();
    [NotMapped]
    public List<string>? HasBeenApprovedByNames { get; set; } = new List<string>();
    public List<DateTime>? ApprovedDates { get; set; } = new List<DateTime>();
    public List<int>? DocumentIds { get; set; } = new List<int>();

    [NotMapped]
    public List<string>? DocumentNames { get; set; } = new List<string>();
    [NotMapped]
    public List<string>? DocumentUrls { get; set; } = new List<string>();
}