using portal.Enums;

namespace portal.Models;

public class ApprovalWorkflowNode : BaseModel
{
    public string Name { get; set; } = string.Empty;

    public int SenderId { get; set; }
    public string? SenderName { get; set; }
    public string? SenderMessage { get; set; }

    public int ReceiverId { get; set; }
    public string? ReceiverName { get; set; }
    public string? ReceiverMessage { get; set; }

    public GeneralWorkflowStatusType ApprovalStatus { get; set; }
    public DateTime? ApprovalDate { get; set; }
    public string? ApprovalComment { get; set; }

    public int? Order { get; set; }

    // Many-to-one back to the parent workflow
    public int GeneralWorkflowId { get; set; }
    public GeneralWorkflow GeneralWorkflow { get; set; } = null!;
    public ICollection<WorkflowNodeDocument> WorkflowNodeDocuments { get; set; } =
        new List<WorkflowNodeDocument>();
}
