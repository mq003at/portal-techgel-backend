using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

namespace portal.Configuration;

public class LeaveRequestWorkflowConfiguration : BaseWorkflowConfiguration<LeaveRequestWorkflow>
{
    public override void Configure(EntityTypeBuilder<LeaveRequestWorkflow> builder)
    {
        base.Configure(builder);

        builder.Property(w => w.Reason).IsRequired().HasMaxLength(1000);
        builder.Property(w => w.RejectReason).IsRequired(false).HasMaxLength(1000);

        builder.HasMany(w => w.WorkflowParticipants)
            .WithOne()
            .HasForeignKey("WorkflowId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(w => w.DocumentAssociations)
            .WithOne()
            .HasForeignKey(da => da.EntityId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}