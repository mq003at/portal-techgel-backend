using System.ComponentModel.DataAnnotations.Schema;

namespace portal.Models;

public class GatePassNodes : BaseWorkflowNode
{
        public GatePassStepType StepType { get; set; }
}