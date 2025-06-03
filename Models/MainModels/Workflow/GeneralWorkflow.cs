using System.ComponentModel.DataAnnotations.Schema;
using portal.Enums;

namespace portal.Models;

public class GeneralWorkflow : BaseModel
{
    public GeneralWorkflowInfo GeneralWorkflowInfo { get; set; } = new();

    public ICollection<int> ApprovalWorkflowNodesIds { get; set; } = new List<int>();
    public ICollection<ApprovalWorkflowNode> ApprovalWorkflowNodes { get; set; } =
        new List<ApprovalWorkflowNode>();
}

/// <summary>
/// Lightweight DTO-like grouping for workflow metadata.
/// </summary>
public class GeneralWorkflowInfo
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public GeneralWorkflowStatusType Status { get; set; }
    // public GeneralWorkflowLogicType WorkflowLogic { get; set; }

    public ICollection<int> ApprovedByIds { get; set; } = new List<int>();
    [NotMapped]
    public ICollection<string> ApprovedByNames { get; set; } = new List<string>();

    public ICollection<int> DraftedByIds { get; set; } = new List<int>();
    [NotMapped]
    public ICollection<string> DraftedByNames { get; set; } = new List<string>();

    // public int? Quota { get; set; }
}
