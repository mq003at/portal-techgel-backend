using System.ComponentModel.DataAnnotations.Schema;

namespace portal.Models;

public class ScheduleInfo : BaseModelWithOnlyId
{
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    // Weekday schedule
    public TimeSpan WeekdayStartTime { get; set; } = new(8, 0, 0); // 08:00
    public TimeSpan WeekdayEndTime { get; set; } = new(17, 0, 0);  // 17:00

    // Saturday schedule (if any)
    public bool WorksOnSaturday { get; set; } = true;
    public TimeSpan? SaturdayStartTime { get; set; } = new(8, 0, 0);
    public TimeSpan? SaturdayEndTime { get; set; } = new(12, 0, 0);

    public bool IsRemoteEligible { get; set; } = false;

    // Optional: calculated field (not stored in DB)
    [NotMapped]
    public double TotalWeeklyHours =>
        ((WeekdayEndTime - WeekdayStartTime).TotalHours * 5) +
        (WorksOnSaturday && SaturdayStartTime.HasValue && SaturdayEndTime.HasValue
            ? (SaturdayEndTime.Value - SaturdayStartTime.Value).TotalHours
            : 0);
}