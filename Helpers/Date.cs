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
        int startDayNightType,
        DateTime endDate,
        int endDayNightType
    )
    {
        if (startDate > endDate)
            throw new ArgumentException("Start date must be before or equal to end date");

        double totalDays = 0;

        for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
        {
            // Sunday: skip
            if (date.DayOfWeek == DayOfWeek.Sunday)
                continue;

            // Saturday: half day by default
            double dayValue = date.DayOfWeek == DayOfWeek.Saturday ? 0.5 : 1.0;

            bool isStart = date == startDate.Date;
            bool isEnd = date == endDate.Date;

            // Special case: same day
            if (isStart && isEnd)
            {
                if (startDayNightType == 1 && endDayNightType == 1)
                    dayValue = 1.0; // Full day leave: morning to evening
                else if (startDayNightType == 1 && endDayNightType == 0)
                    dayValue = 0.5; // Morning only
                else if (startDayNightType == 0 && endDayNightType == 0)
                    dayValue = 0.5; // Morning only
                else if (startDayNightType == 0 && endDayNightType == 1)
                    dayValue = 1.0; // Morning to evening
            }
            else
            {
                if (isStart && startDayNightType == 1)
                    dayValue -= 0.5;

                if (isEnd && endDayNightType == 0)
                    dayValue -= 0.5;
            }

            totalDays += Math.Max(dayValue, 0);
        }

        return totalDays;
    }
}
