using portal.Enums;

namespace portal.DTOs;

public class WorkflowNodeParticipantDTO : BaseModelWithOnlyIdDTO
{
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;

    public int WorkflowNodeId { get; set; }
    public string WorkflowNodeName { get; set; } = string.Empty;

    public WorkflowParticipantRoleType RaciRole { get; set; }
    public int NodeStep { get; set; }

    public DateTime? ApprovalDate { get; set; }
    public DateTime? ApprovalDeadline { get; set; }

    public ApprovalStatusType ApprovalStatus { get; set; }
    public TimeSpan TAT { get; set; } = TimeSpan.Zero;
}

public class WorkflowNodeParticipantCreateDTO : BaseModelWithOnlyIdCreateDTO
{
    public int EmployeeId { get; set; }
    public int WorkflowNodeId { get; set; }
    public WorkflowParticipantRoleType RaciRole { get; set; }
    public ApprovalStatusType ApprovalStatus { get; set; } = ApprovalStatusType.PENDING;
    public int NodeStep { get; set; }
    public DateTime? ApprovalDeadline { get; set; }
}

public class WorkflowNodeParticipantUpdateDTO : BaseModelWithOnlyIdUpdateDTO
{
    public int EmployeeId { get; set; }
    public int WorkflowNodeId { get; set; }
    public WorkflowParticipantRoleType RaciRole { get; set; }
    public int NodeStep { get; set; }
    public DateTime? ApprovalDeadline { get; set; }
    public ApprovalStatusType? ApprovalStatus { get; set; } 
}