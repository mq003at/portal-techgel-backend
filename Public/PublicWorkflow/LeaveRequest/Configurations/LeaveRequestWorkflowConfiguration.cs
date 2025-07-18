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

        builder
            .HasOne(l => l.Employee)
            .WithMany()
            .HasForeignKey(l => l.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(l => l.TotalDays).IsRequired();
        builder.Property(l => l.EmployeeAnnualLeaveTotalDays).IsRequired();
        builder.Property(l => l.FinalEmployeeAnnualLeaveTotalDays).IsRequired();
        builder.Property(l => l.EmployeeCompensatoryLeaveTotalDays).IsRequired();
        builder.Property(l => l.FinalEmployeeCompensatoryLeaveTotalDays).IsRequired();

        builder.Property(l => l.Notes).HasMaxLength(1000);
        builder.Property(l => l.RejectReason).HasMaxLength(1000);

        builder.Property(l => l.StartDate).IsRequired();

        builder.Property(l => l.EndDate).IsRequired();

        builder.Property(l => l.StartDateDayNightType).HasConversion<int>().IsRequired();

        builder.Property(l => l.EndDateDayNightType).HasConversion<int>().IsRequired();

        builder.Property(l => l.LeaveApprovalCategory).HasConversion<int>().IsRequired();
        builder.Property(l => l.AssigneeDetails).HasColumnType("integer[]");
        builder
            .Property(l => l.AssigneeNames)
            .HasConversion(
                v => string.Join(",", v ?? new List<string>()),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
            )
            .Metadata.SetValueComparer(GlobalValueComparers.StringListComparer);
    }
}
