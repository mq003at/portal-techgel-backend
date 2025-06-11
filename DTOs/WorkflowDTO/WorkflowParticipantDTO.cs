using portal.Enums;

namespace portal.DTOs;

public class WorkflowParticipantDTO : BaseModelWithOnlyIdDTO
{
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;

    public int WorkflowNodeId { get; set; }
    public string WorkflowNodeName { get; set; } = string.Empty;

    public WorkflowParticipantRoleType RaciRole { get; set; }
    public string WorkflowRole { get; set; } = string.Empty;

    public int NodeStep { get; set; }

    public DateTime? ApprovalDate { get; set; }
    public DateTime? ApprovalDeadline { get; set; }

    public bool? IsApproved { get; set; }
    public bool? IsRejected { get; set; }
    public TimeSpan TAT { get; set; } = TimeSpan.Zero;
}

public class WorkflowParticipantCreateDTO : BaseModelWithOnlyIdCreateDTO
{
    public int EmployeeId { get; set; }
    public int WorkflowNodeId { get; set; }
    public WorkflowParticipantRoleType RaciRole { get; set; }
    public string WorkflowRole { get; set; } = string.Empty;
    public int NodeStep { get; set; }
    public DateTime? ApprovalDeadline { get; set; }
}

public class WorkflowParticipantUpdateDTO : BaseModelWithOnlyIdUpdateDTO
{
    public int EmployeeId { get; set; }
    public int WorkflowNodeId { get; set; }
    public WorkflowParticipantRoleType RaciRole { get; set; }
    public string WorkflowRole { get; set; } = string.Empty;
    public int NodeStep { get; set; }
    public DateTime? ApprovalDeadline { get; set; }
    public bool? IsApproved { get; set; }
    public bool? IsRejected { get; set; }
}