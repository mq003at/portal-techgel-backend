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

        builder.HasMany(x => x.LeaveRequestNodes)
            .WithOne()
            .HasForeignKey("LeaveRequestWorkflowId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}