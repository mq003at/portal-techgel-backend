using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public static class AppTimeZoneConverter
{
    public static string DefaultTimeZoneId { get; set; } = "Asia/Ho_Chi_Minh";

    public static IHttpContextAccessor? HttpContextAccessor { get; set; }

    public static void Configure(IHttpContextAccessor accessor)
    {
        HttpContextAccessor = accessor;
    }

    public static TimeZoneInfo GetTimeZone()
    {
        try
        {
            var context = HttpContextAccessor?.HttpContext;
            var tzCookie = context?.Request.Cookies["tz"];

            if (!string.IsNullOrWhiteSpace(tzCookie))
                return TimeZoneInfo.FindSystemTimeZoneById(tzCookie);
        }
        catch
        {
            // Ignore and fall back
        }

        return TimeZoneInfo.FindSystemTimeZoneById(DefaultTimeZoneId);
    }

    public static readonly ValueConverter<DateTimeOffset, DateTimeOffset> DateTimeOffsetConverter =
        new ValueConverter<DateTimeOffset, DateTimeOffset>(
            toDb => toDb.ToUniversalTime(),
            fromDb => TimeZoneInfo.ConvertTime(fromDb, GetTimeZone())
        );

    public static readonly ValueConverter<DateTime, DateTime> DateTimeConverter =
        new ValueConverter<DateTime, DateTime>(
            toDb => DateTime.SpecifyKind(toDb, DateTimeKind.Utc),
            fromDb =>
                TimeZoneInfo.ConvertTimeFromUtc(
                    DateTime.SpecifyKind(fromDb, DateTimeKind.Utc),
                    GetTimeZone()
                )
        );
}
