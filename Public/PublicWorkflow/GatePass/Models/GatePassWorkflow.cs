using System.ComponentModel.DataAnnotations.Schema;

namespace portal.Models;

public class GatePassWorkflow : BaseWorkflow
{
    public string Reason { get; set; } = null!;
    public DateTimeOffset GatePassStartTime { get; set; }
    public DateTimeOffset GatePassEndTime { get; set; }

    public virtual List<GatePassNode> GatePassNodes { get; set; } = new List<GatePassNode>();
}
