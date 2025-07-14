namespace portal.Models;

public abstract class BaseWorkflowEvent : BaseIntegrationEvent
{
    public string WorkflowId { get; set; } = null!;
    public string WorkflowType { get; set; } = null!;
    public int EmployeeId { get; set; }
    public string ApproverName { get; set; } = string.Empty;
}

public class ApprovalEvent : BaseWorkflowEvent
{
    public string Status { get; set; } = "APPROVED";
    public DateTime ApprovedAt { get; set; }
}

public class RejectEvent : BaseWorkflowEvent
{
    public string Status { get; set; } = "REJECTED";
    public DateTime RejectedAt { get; set; }
    public string Reason { get; set; } = string.Empty;
}

public class CompleteEvent : BaseWorkflowEvent
{
    public DateTime CompletedAt { get; set; }
    public string Status { get; set; } = "COMPLETED";
}

public class CreateEvent : BaseWorkflowEvent
{
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; } = "CREATED";
    public string EmployeeName { get; set; } = null!;
    public string AssigneeDetails { get; set; } = null!;
}
