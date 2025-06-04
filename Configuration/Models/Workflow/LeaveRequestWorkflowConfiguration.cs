using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

namespace portal.Configuration;

public class LeaveRequestWorkflowConfiguration : BaseModelConfiguration<LeaveRequestWorkflow>
{
    public override void Configure(EntityTypeBuilder<LeaveRequestWorkflow> builder)
    {
        base.Configure(builder);

        builder.ToTable("LeaveRequestWorkflows");

        builder.Property(w => w.Name).HasMaxLength(255).IsRequired();
        builder.Property(w => w.Description).HasMaxLength(1000);

        builder.Property(w => w.Status)
               .HasConversion<int>()
               .IsRequired();

        builder.Property(w => w.Reason).HasMaxLength(1000);
        builder.Property(w => w.StartDate).IsRequired();
        builder.Property(w => w.EndDate).IsRequired();
        builder.Property(w => w.EmployeeId).IsRequired();

        // Mapping array columns (no HasConversion needed)
        builder.Property(w => w.ReceiverIds).HasColumnType("integer[]");
        builder.Property(w => w.DraftedByIds).HasColumnType("integer[]");
        builder.Property(w => w.HasBeenApprovedByIds).HasColumnType("integer[]");
        builder.Property(w => w.ApprovedDates).HasColumnType("timestamptz[]");

        // Configure buoi sang hay chieu
        builder.Property(n => n.StartDateDayNightType)
            .HasConversion<int>()
            .IsRequired();
        builder.Property(n => n.EndDateDayNightType)
            .HasConversion<int>()
            .IsRequired();

        builder.HasMany(w => w.LeaveRequestNodes)
               .WithOne(n => n.LeaveRequestWorkflow)
               .HasForeignKey(n => n.LeaveRequestWorkflowId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}