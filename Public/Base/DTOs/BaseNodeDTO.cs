using DocumentFormat.OpenXml.Math;
using portal.Enums;
using portal.Models;

namespace portal.DTOs;

public abstract class WorkflowNodeDTO : BaseModelDTO
{
    public string Name { get; set; } = null!;
    public int WorkflowId { get; set; }
    public GeneralWorkflowStatusType Status { get; set; }
    public LeaveApprovalStepType StepType { get; set; }
    public WorkflowNodeApprovalLogic ApprovalLogic { get; set; }
    public string Description { get; set; } = null!;

    // Navigation fields
    public List<WorkflowNodeParticipantDTO> WorkflowNodeParticipants { get; set; } = new();
}

public abstract class WorkflowNodeCreateDTO : BaseModelCreateDTO
{
    public string Name { get; set; } = null!;

    // Only Ids for creation
    public List<WorkflowNodeParticipantCreateDTO>? WorkflowNodeParticipant { get; set; }
}

public abstract class WorkflowNodeUpdateDTO : BaseModelUpdateDTO
{
    // Only Ids for update
    public List<WorkflowNodeParticipantUpdateDTO>? WorkflowNodeParticipants { get; set; } = new();
}

public class ApproverDTO
{
    public int ApproverId { get; set; }
}

public class ApproveWithCommentDTO : ApproverDTO
{
    public string? Comment { get; set; } = null!;
}

public class RejectDTO
{
    public int ApproverId { get; set; }
    public string RejectReason { get; set; } = null!;
}
