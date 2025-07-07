namespace portal.DTOs;

using System.ComponentModel.DataAnnotations;

public class GeneralProposalWorkflowDTO : BaseWorkflowDTO
{
    [Required]
    public string About { get; set; } = null!;

    [Required]
    public string Comment { get; set; } = null!;

    public string ProjectName { get; set; } = null!;

    public string Reason { get; set; } = null!;

    public string Proposal { get; set; } = null!;
    public int ApproverId { get; set; }
    public string ApproverMainId { get; set; } = null!;
    public string ApproverName { get; set; } = null!;
    public List<GeneralProposalNodeDTO> GeneralProposalNodes { get; set; } = new();
}

public class GeneralProposalWorkflowCreateDTO : BaseWorkflowCreateDTO
{
    [Required]
    public string About { get; set; } = null!;

    [Required]
    public string Description { get; set; } = null!;

    [Required]
    public string ProjectName { get; set; } = null!;

    [Required]
    public string Reason { get; set; } = null!;

    [Required]
    public string Proposal { get; set; } = null!;

    [Required]
    public int ApproverId { get; set; }
}

public class GeneralProposalWorkflowUpdateDTO : BaseWorkflowUpdateDTO
{
    public string? About { get; set; } = null!;
    public string? ProjectName { get; set; } = null!;
    public string? Description { get; set; } = null!;
    public string? Reason { get; set; } = null!;
    public string? Proposal { get; set; } = null!;
    public int? ApproverId { get; set; }
}
