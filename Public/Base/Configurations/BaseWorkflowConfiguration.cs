using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

namespace portal.Configuration;

public abstract class BaseWorkflowConfiguration<TWorkflow> : BaseModelConfiguration<TWorkflow>
    where TWorkflow : BaseWorkflow
{
    public override void Configure(EntityTypeBuilder<TWorkflow> builder)
    {
        base.Configure(builder);

        builder.Property(w => w.Name).IsRequired().HasMaxLength(255);

        builder.Property(w => w.Description).IsRequired().HasMaxLength(1000);

        builder.Property(w => w.Status).IsRequired().HasConversion<string>();

        builder.Property(w => w.RejectReason).HasMaxLength(1000).IsRequired(false);
        builder.Property(w => w.Comment).HasMaxLength(1000).IsRequired(false);
        builder.Property(w => w.SenderId).IsRequired();
        builder
            .Property(w => w.ParticipantIds)
            .HasConversion(
                v => string.Join(",", v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList()
            )
            .Metadata.SetValueComparer(GlobalValueComparers.IntListComparer);

        builder
            .HasOne(l => l.Sender) // navigation property on LeaveRequestWorkflow
            .WithMany() // no reverse navigation on Employee
            .HasForeignKey(l => l.SenderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
