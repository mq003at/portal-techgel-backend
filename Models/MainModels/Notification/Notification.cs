using System.ComponentModel.DataAnnotations;
using portal.Models;

public class Notification : BaseModel
{
    [Required]
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;
    public long NotificationCategoryId { get; set; }
    public NotificationCategory NotificationCategory { get; set; } = null!;

    [Required]
    public string Title { get; set; } = null!;

    [Required]
    public string Message { get; set; } = null!;
    public string? Url { get; set; }
    public DateTime? ReadAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public UrgencyLevel UrgencyLevel { get; set; }
}

/// <summary>
/// Six colors for 5 urgency level.
/// 0: Can be put on hold indefinitely -> grey
/// 1: Can be put on hold for a long time -> green
/// 2: Need to be done soon, caution. -> yellow
/// 3: Need to be done soon, but not immediately -> orange
/// 4: Need to be done immediately -> red
/// </summary>
public enum UrgencyLevel
{
    LOW = 0,
    MEDIUM = 1,
    HIGH = 2,
    URGENT = 3,
    CRITICAL = 4
}
