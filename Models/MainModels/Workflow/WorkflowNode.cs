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
}