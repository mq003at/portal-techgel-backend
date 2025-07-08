using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

namespace portal.Configuration;

public class GeneralProposalWorkflowConfiguration
    : BaseWorkflowConfiguration<GeneralProposalWorkflow>
{
    public override void Configure(EntityTypeBuilder<GeneralProposalWorkflow> builder)
    {
        base.Configure(builder);

        builder.Property(w => w.Reason).IsRequired().HasMaxLength(1000);
        builder.Property(w => w.RejectReason).IsRequired(false).HasMaxLength(1000);

        builder
            .HasOne(l => l.Approver)
            .WithMany()
            .HasForeignKey(l => l.ApproverId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(l => l.About).IsRequired().HasMaxLength(500);
        builder.Property(l => l.Comment).IsRequired().HasMaxLength(1000);
        builder.Property(l => l.ProjectName).HasMaxLength(500);
        builder.Property(l => l.Reason).HasMaxLength(1000);
        builder.Property(l => l.Proposal).HasMaxLength(2000);
    }
}
