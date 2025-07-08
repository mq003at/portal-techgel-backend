namespace portal.Models;

public class DelayedNotificationEvent
{
    public long NotificationCategoryId { get; set; }
    public int EmployeeId { get; set; }
    public string EventPayload { get; set; } = string.Empty;
    public string TriggerEvent { get; set; } = null!;
}
