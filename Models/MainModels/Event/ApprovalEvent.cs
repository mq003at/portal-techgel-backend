namespace portal.Models;

public abstract class BaseWorkflowEvent : BaseIntegrationEvent
{
    public int WorkflowId { get; set; }
    public WorkflowType WorkflowType { get; set; }
    public int EmployeeId { get; set; }
    public string ApproverName { get; set; } = string.Empty;
}

public class ApprovalEvent : BaseWorkflowEvent
{
    public string Status { get; set; } = "APPROVED";
    public DateTime ApprovedAt { get; set; }
}

public class RejectEvent : BaseIntegrationEvent
{
    public string Status { get; set; } = "REJECTED";
    public DateTime RejectedAt { get; set; }
    public string Reason { get; set; } = string.Empty;
}
