// using portal.Enums;

// namespace portal.Models;

// public class ApprovalWorkflowNode : BaseModel
// {
//     public string Name { get; set; } = string.Empty;

//     public int SenderId { get; set; }
//     public string? SenderName { get; set; }
//     public string? SenderMessage { get; set; }

//     public List<int> ReceiverIds { get; set; } = new List<int>();
//     public List<string>? ReceiverNames { get; set; } = new List<string>();
//     public List<string>? ReceiverMessages { get; set; } = new List<string>();

//     public GeneralWorkflowStatusType Status { get; set; }
//     public DateTime? ApprovalDate { get; set; }
//     public List<int> ApprovalCommentIds { get; set; } = new List<int>();
//     public List<string>? ApprovalComments { get; set; } = new List<string>();

//     public int? Order { get; set; }

//     // Many-to-one back to the parent workflow
//     public int GeneralWorkflowId { get; set; }
//     public GeneralWorkflow GeneralWorkflow { get; set; } = null!;
//     public List<Document> Documents { get; set; } = new List<Document>();
//     public List<int> DocumentIds { get; set; } = new List<int>();
// }
