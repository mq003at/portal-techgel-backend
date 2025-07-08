using System.ComponentModel.DataAnnotations.Schema;

namespace portal.Models;

public class GatePassNode : BaseWorkflowNode
{
    public GatePassStepType StepType { get; set; }
    public virtual GatePassWorkflow Workflow { get; set; } = null!;
}
