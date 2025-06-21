using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

namespace portal.Configuration.Models;

public class ScheduleInfoConfiguration
    : BaseModelWithOnlyIdConfiguration<ScheduleInfo>, // inherit your generic config
        IEntityTypeConfiguration<ScheduleInfo>
{
    public override void Configure(EntityTypeBuilder<ScheduleInfo> builder)
    {
        builder.ToTable("ScheduleInfo");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired()
            .ValueGeneratedNever();

        builder.HasOne(s => s.Employee)
               .WithOne(e => e.ScheduleInfo)
               .HasForeignKey<ScheduleInfo>(s => s.EmployeeId)
               .OnDelete(DeleteBehavior.Cascade);

        // Weekday schedule
        builder.Property(s => s.WeekdayStartTime)
               .IsRequired();

        builder.Property(s => s.WeekdayEndTime)
               .IsRequired();

        // Saturday schedule
        builder.Property(s => s.WorksOnSaturday)
               .IsRequired();

        builder.Property(s => s.SaturdayStartTime);

        builder.Property(s => s.SaturdayEndTime);

        // Remote eligibility
        builder.Property(s => s.IsRemoteEligible)
               .IsRequired();
        // Ignore calculated field
        builder.Ignore(s => s.TotalWeeklyHours);
    }
}
