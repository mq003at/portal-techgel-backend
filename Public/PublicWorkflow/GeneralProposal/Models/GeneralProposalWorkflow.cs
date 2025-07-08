using System.ComponentModel.DataAnnotations;

namespace portal.Models;

public class GeneralProposalWorkflow : BaseWorkflow
{
    [Required]
    public string About { get; set; } = null!;

    public string ProjectName { get; set; } = null!;

    public string Reason { get; set; } = null!;

    public string Proposal { get; set; } = null!;

    public int ApproverId { get; set; }
    public Employee Approver { get; set; } = null!;
    public List<GeneralProposalNode> GeneralProposalNodes { get; set; } = null!;
}
