using portal.Enums;

namespace portal.DTOs;

public class GatePassNodeDTO : WorkflowNodeDTO
{
    public new GeneralProposalStepType StepType { get; set; }
    public string StepTypeName { get; set; } = null!;
}

public class GatePassNodeCreateDTO : WorkflowNodeCreateDTO
{
    public GatePassStepType StepType { get; set; }
}

public class GatePassNodeUpdateDTO : WorkflowNodeUpdateDTO
{
    public GeneralProposalStepType StepType { get; set; }
}
