// using System.ComponentModel.DataAnnotations;
// using portal.Enums;
// using portal.Models;

// namespace portal.DTOs;

// public abstract class ApprovalWorkflowNodeDTO : BaseDTO<ApprovalWorkflowNode>
// {
//     [Required, StringLength(255)]
//     public string Name { get; set; } = string.Empty;

//     [Required]
//     public int SenderId { get; set; }
//     public string? SenderName { get; set; }
//     public string? SenderMessage { get; set; }

//     [Required]
//     public List<int> ReceiverIds { get; set; } = new List<int>();
//     public List<string>? ReceiverNames { get; set; } = new List<string>();
//     public List<string> ReceiverMessages { get; set; } = new List<string>();

//     [Required]
//     public GeneralWorkflowStatusType ApprovalStatus { get; set; }
//     public DateTime? ApprovalDate { get; set; }
//     public string? ApprovalComment { get; set; }
//     public List<int> DocumentIds { get; set; } = new();
//     public List<string> DocumentNames { get; set; } = new();
//     public int? Order { get; set; }
//     public int GeneralWorkflowId { get; set; }
// }

// /// <summary>
// /// DTO for creating or updating an approval node.
// /// </summary>
// public class CreateApprovalWorkflowNodeDTO : BaseDTO<ApprovalWorkflowNode>
// {
//     [Required, StringLength(255)]
//     public string Name { get; set; } = string.Empty;

//     [Required]
//     public int SenderId { get; set; }
//     public string? SenderName { get; set; }
//     public string? SenderMessage { get; set; }

//     [Required]
//     public List<int> ReceiverIds { get; set; } = new List<int>();

//     [Required]
//     public GeneralWorkflowStatusType ApprovalStatus { get; set; }
//     public DateTime? ApprovalDate { get; set; }
//     public string? ApprovalComment { get; set; }

//     public int? Order { get; set; }

//     // Optional category used as a sub-folder for file storage
//     public string? Category { get; set; }

//     // Optional file attachments
//     public List<IFormFile>? Files { get; set; }
//     public List<int>? DocumentIds { get; set; }

//     [Required]
//     public int GeneralWorkflowId { get; set; }
// }

// public class UpdateApprovalWorkflowNodeDTO : BaseDTO<ApprovalWorkflowNode>
// {
//     public string? Name { get; set; } = string.Empty;

//     [Required]
//     public List<int> ReceiverIds { get; set; } = new List<int>();
//     [Required]
//     public GeneralWorkflowStatusType ApprovalStatus { get; set; }
//     public DateTime? ApprovalDate { get; set; }
//     public string? ApprovalComment { get; set; }
//     public List<int> DocumentIds { get; set; }
//         = new List<int>();

//     public int? Order { get; set; }
// }

// public class DeleteFilesFromApprovalWorkflowNodesDTO : BaseDTO<ApprovalWorkflowNode>
// {
//     [Required]
//     public List<int> DocumentIds { get; set; } = new List<int>();
// }

// // DTOs for when files are modified in the approval workflow node
// public class ApprovalWorkflowNodeFileResultDTO : BaseDTO<ApprovalWorkflowNode>
// {
//     public int NodeId { get; set; }

//     public List<int> DocumentIds { get; set; } = new();

//     public List<string> DocumentNames { get; set; } = new();

//     public List<string> DocumentUrls { get; set; } = new();
// }
