namespace portal.Helpers;

public static class DateHelper
{
    // Check if a date is null or default
    public static bool IsNullOrDefault(this DateTime? date)
    {
        return !date.HasValue || date.Value == default;
    }

    // Check if a date is not null and not default
    public static bool IsNotNullOrDefault(this DateTime? date)
    {
        return !IsNullOrDefault(date);
    }

    // Check if a date is in the future
    public static bool IsInFuture(this DateTime? date)
    {
        return date.HasValue && date.Value > DateTime.Now;
    }

    public static double CalculateLeaveDays(
        DateTime startDate,
        DayNightEnum startDayNightType,
        DateTime endDate,
        DayNightEnum endDayNightType
    )
    {
        if (startDate > endDate)
            throw new ArgumentException("Start date must be before or equal to end date");

        double WorkingDayWeight(DateTime date)
        {
            return date.DayOfWeek switch
            {
                DayOfWeek.Sunday => 0.0,
                DayOfWeek.Saturday => 0.5,
                _ => 1.0
            };
        }

        double totalDays = 0.0;

        // Trả về ngày chỉ có phần ngày
        DateTime current = startDate.Date;
        DateTime end = endDate.Date;

        // Cộng toàn bộ số ngày làm việc giữa 2 ngày
        while (current <= end)
        {
            totalDays += WorkingDayWeight(current);
            current = current.AddDays(1);
        }

        DayOfWeek startDay = startDate.DayOfWeek;
        DayOfWeek endDay = endDate.DayOfWeek;

        // Trừ nửa ngày nếu bắt đầu là buổi chiều (Night)
        if (startDayNightType == DayNightEnum.Night)
        {
            if (startDay >= DayOfWeek.Monday && startDay <= DayOfWeek.Friday)
            {
                totalDays -= 0.5;
            }
            else if (startDay == DayOfWeek.Saturday)
            {
                // Thứ bảy chỉ làm sáng → bắt đầu từ chiều => không có buổi chiều => chỉ giữ lại sáng
                totalDays = Math.Max(totalDays - 0.5, 0);
            }
        }

        // Trừ nửa ngày nếu kết thúc là buổi sáng (Day)
        if (endDayNightType == DayNightEnum.Day)
        {
            if (endDay >= DayOfWeek.Monday && endDay <= DayOfWeek.Friday)
            {
                totalDays -= 0.5;
            }
            // Thứ bảy sáng thì vẫn giữ lại như cũ, không trừ thêm
        }

        return Math.Max(totalDays, 0);
    }
}

