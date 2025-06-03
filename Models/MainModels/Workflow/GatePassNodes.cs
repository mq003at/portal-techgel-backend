using System.ComponentModel.DataAnnotations.Schema;

namespace portal.Models;

public class GatePassNodes : WorkflowNode
{
    public int GatePassId { get; set; }
    [NotMapped]
    public GatePass GatePass { get; set; } = new();
}