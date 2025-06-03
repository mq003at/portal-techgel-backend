// using System.ComponentModel.DataAnnotations;
// using portal.Enums;
// using portal.Models;

// namespace portal.DTOs;

// public class GeneralWorkflowInfoDTO
// {
//     [Required, StringLength(255)]
//     public string Name { get; set; } = string.Empty;

//     public string? Description { get; set; }

//     [Required]
//     public GeneralWorkflowStatusType Status { get; set; }

//     [Required]
//     public GeneralWorkflowLogicType WorkflowLogic { get; set; }

//     public int? Quota { get; set; }

//     // Collections of approver and drafter metadata (IDs, names, signatures)
//     public List<int> ApprovedByIds { get; set; } = new();
//     public List<string> ApprovedByNames { get; set; } = new();
//     public List<int> DraftedByIds { get; set; } = new();
//     public List<string> DraftedByNames { get; set; } = new();
// }

// public class GeneralWorkflowDTO : BaseDTO<GeneralWorkflow>
// {
//     public GeneralWorkflowInfoDTO GeneralInfo { get; set; } = new();
//     public List<int> ApprovalWorkflowNodesIds { get; set; } = new();
//     public List<ApprovalWorkflowNodeDTO> ApprovalWorkflowNodes { get; set; } = new();
// }

// /// <summary>
// /// DTO for creating a new workflow.
// /// </summary>
// public class CreateGeneralWorkflowDTO : BaseDTO<GeneralWorkflow>
// {
//     [Required]
//     public GeneralWorkflowInfoDTO GeneralInfo { get; set; } = new();
//     public List<CreateApprovalWorkflowNodeDTO> ApprovalWorkflowNodes { get; set; } = new();
// }

// /// <summary>
// /// DTO for updating an existing workflow.
// /// </summary>
// public class UpdateGeneralWorkflowDTO : BaseDTO<GeneralWorkflow>
// {
//     [Required]
//     public GeneralWorkflowInfoDTO GeneralInfo { get; set; } = new();
//     public List<CreateApprovalWorkflowNodeDTO> ApprovalWorkflowNodes { get; set; } = new();
// }
