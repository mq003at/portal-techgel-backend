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

        // List<int> mappings
        builder.Property(w => w.ReceiverIds)
            .HasConversion(
                v => string.Join(",", v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList()
            );

        builder.Property(w => w.DraftedByIds)
            .HasConversion(
                v => string.Join(",", v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList()
            );

        builder.Property(w => w.HasBeenApprovedByIds)
            .HasConversion(
                v => string.Join(",", v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList()
            );

        builder.Property(w => w.ApprovedDates)
            .HasConversion(
                v => string.Join(",", v.Select(d => d.ToString("o"))),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(DateTime.Parse).ToList()
            );

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
