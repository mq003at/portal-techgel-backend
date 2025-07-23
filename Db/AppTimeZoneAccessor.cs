public static class AppTimeZoneAccessor
{
    private static readonly AsyncLocal<string?> _userTimeZone = new();

    private static string _defaultTimeZoneId = "Asia/Ho_Chi_Minh";

    public static string CurrentTimeZoneId => _userTimeZone.Value ?? _defaultTimeZoneId;

    public static void SetUserTimeZone(string? timeZoneId)
    {
        if (string.IsNullOrWhiteSpace(timeZoneId))
        {
            _userTimeZone.Value = null;
            return;
        }

        _ = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId); // will throw if invalid
        _userTimeZone.Value = timeZoneId;
    }

    public static void SetDefaultTimeZone(string timeZoneId)
    {
        _ = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId); // validate
        _defaultTimeZoneId = timeZoneId;
    }

    public static TimeZoneInfo CurrentTimeZone =>
        TimeZoneInfo.FindSystemTimeZoneById(CurrentTimeZoneId);
}
