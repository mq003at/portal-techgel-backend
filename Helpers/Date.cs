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

    public static double CalculateLeaveDays(DateTime startDate, int startDayNightType, DateTime endDate, int endDayNightType)
    {
        // Ensure startDate <= endDate
        if (startDate > endDate)
            throw new ArgumentException("Start date must be before or equal to end date");

        double totalDays = 0;

        for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
        {
            // Sunday: skip
            if (date.DayOfWeek == DayOfWeek.Sunday)
                continue;

            // Saturday: half day
            double dayValue = date.DayOfWeek == DayOfWeek.Saturday ? 0.5 : 1.0;

            // If first day
            if (date == startDate.Date)
            {
                if (startDayNightType == 1) // night
                    dayValue -= 0.5; // Only half day if starting at night
            }

            // If last day
            if (date == endDate.Date)
            {
                if (endDayNightType == 1) // night
                    dayValue -= 0.5; // Only half day if ending at night
            }

            // If both start and end are night and same day: only 0.5
            if (startDate.Date == endDate.Date && startDayNightType == 1 && endDayNightType == 1)
                dayValue = 0.5;

            // Don't double subtract if same day
            totalDays += Math.Max(dayValue, 0);
        }

        return (double)totalDays;
    }
}