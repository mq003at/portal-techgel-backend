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
    public ICollection<int> ApprovedByIds { get; set; } = new List<int>();

    [NotMapped]
    public ICollection<string> ApprovedByNames { get; set; } = new List<string>();

    public ICollection<int> HasBeenApprovedByIds { get; set; } = new List<int>();
    [NotMapped]
    public ICollection<string> HasBeenApprovedByNames { get; set; } = new List<string>();
    public ICollection<DateTime> ApprovedDates { get; set; } = new List<DateTime>();
    public ICollection<int> DocumentIds { get; set; } = new List<int>();

    [NotMapped]
    public ICollection<string> DocumentNames { get; set; } = new List<string>();
    [NotMapped]
    public ICollection<string> DocumentUrls { get; set; } = new List<string>();
}