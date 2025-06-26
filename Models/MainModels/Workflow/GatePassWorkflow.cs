using System.ComponentModel.DataAnnotations.Schema;
namespace portal.Models;

public class GatePassWorkflow : BaseWorkflow
{
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;
    public string Reason { get; set; } = null!;
    public DateTime GatePassStartTime { get; set; }
    public DateTime GatePassEndTime { get; set; }

    public virtual List<GatePassNode> GatePassNodes { get; set; } = new List<GatePassNode>();
}