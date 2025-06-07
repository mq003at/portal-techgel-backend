using System.ComponentModel.DataAnnotations.Schema;
using portal.Enums;

namespace portal.Models;

abstract public class WorkflowNode : BaseModel
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public GeneralWorkflowStatusType Status { get; set; }
    public int SenderId { get; set; }
    public Employee Sender { get; set; } = null!;
    public List<int> ApprovedByIds { get; set; } = new List<int>();

    public List<int> HasBeenApprovedByIds { get; set; } = new List<int>();

    public List<DateTime> ApprovedDates { get; set; } = new List<DateTime>();
    public List<int> DocumentIds { get; set; } = new List<int>();
}