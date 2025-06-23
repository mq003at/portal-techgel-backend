namespace portal.DTOs;

public class ScheduleInfoDTO : BaseModelWithOnlyIdDTO
{
    public int EmployeeId { get; set; }

    public TimeSpan WeekdayStartTime { get; set; }
    public TimeSpan WeekdayEndTime { get; set; }

    public bool WorksOnSaturday { get; set; }
    public TimeSpan? SaturdayStartTime { get; set; }
    public TimeSpan? SaturdayEndTime { get; set; }

    public bool IsRemoteEligible { get; set; }

    // Optional: calculated field (can be calculated client-side too)
    public double TotalWeeklyHours { get; set; }
}

public class CreateScheduleInfoDTO
{
    public int EmployeeId { get; set; }

    public TimeSpan WeekdayStartTime { get; set; } = new(8, 0, 0);
    public TimeSpan WeekdayEndTime { get; set; } = new(17, 0, 0);

    public bool WorksOnSaturday { get; set; } = true;
    public TimeSpan? SaturdayStartTime { get; set; } = new(8, 0, 0);
    public TimeSpan? SaturdayEndTime { get; set; } = new(12, 0, 0);

    public bool IsRemoteEligible { get; set; } = false;
}

public class UpdateScheduleInfoDTO
{
    public TimeSpan? WeekdayStartTime { get; set; }
    public TimeSpan? WeekdayEndTime { get; set; }

    public bool? WorksOnSaturday { get; set; }
    public TimeSpan? SaturdayStartTime { get; set; }
    public TimeSpan? SaturdayEndTime { get; set; }

    public bool? IsRemoteEligible { get; set; }
}