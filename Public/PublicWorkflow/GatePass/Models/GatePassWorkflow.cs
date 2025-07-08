using System.ComponentModel.DataAnnotations.Schema;

namespace portal.Models;

public class GatePassWorkflow : BaseWorkflow
{
    public string Reason { get; set; } = null!;
    public DateTime GatePassStartTime { get; set; }
    public DateTime GatePassEndTime { get; set; }

    public virtual List<GatePassNode> GatePassNodes { get; set; } = new List<GatePassNode>();
}
