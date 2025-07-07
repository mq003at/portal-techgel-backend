namespace portal.Models;

using System.ComponentModel.DataAnnotations;
using portal.Enums;

public class GeneralProposalNode : BaseWorkflowNode
{
    public GeneralProposalStepType StepType { get; set; }

    [Required]
    public virtual GeneralProposalWorkflow Workflow { get; set; } = null!;
}
