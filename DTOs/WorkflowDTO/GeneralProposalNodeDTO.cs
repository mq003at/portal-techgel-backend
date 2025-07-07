using portal.Enums;

namespace portal.DTOs;


public class GeneralProposalNodeDTO : WorkflowNodeDTO
{
    public new GeneralProposalStepType StepType { get; set; }
    public string StepTypeName { get; set; } = null!;
}

public class GeneralProposalNodeCreateDTO : WorkflowNodeCreateDTO
{
    public GeneralProposalStepType StepType { get; set; }
}

public class GeneralProposalNodeUpdateDTO : WorkflowNodeUpdateDTO
{
    public GeneralProposalStepType StepType { get; set; }
}
