using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

namespace portal.Configuration;

public class GeneralProposalNodeConfiguration : BaseWorkflowNodeConfiguration<GeneralProposalNode>
{
    public override void Configure(EntityTypeBuilder<GeneralProposalNode> builder)
    {
        base.Configure(builder);
        builder.Property(n => n.StepType).IsRequired().HasConversion<string>();

        // Common navigation: workflow
        builder
            .HasOne(n => n.Workflow)
            .WithMany(w => w.GeneralProposalNodes)
            .HasForeignKey(n => n.WorkflowId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
