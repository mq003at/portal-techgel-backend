using System.ComponentModel.DataAnnotations;
using portal.Models;

public class Notification : BaseModel
{
    // == Who receives it ==
    [Required]
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    // == Rule origin ==
    public long NotificationCategoryId { get; set; }
    public NotificationCategory NotificationCategory { get; set; } = null!;

    // == Display content ==
    [Required]
    public string Title { get; set; } = null!;

    [Required]
    public string Message { get; set; } = null!;
    public string? Url { get; set; } // Optional deep link to workflow page

    // == Entity context (optional polymorphic pointer to source) ==
    public string? TriggerEntity { get; set; } // e.g. "LeaveRequest"
    public int? TriggerEntityId { get; set; }

    // == Status ==
    public DateTime? ReadAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public UrgencyLevel UrgencyLevel { get; set; }

    // == Delivery metadata ==
    public DeliveryChannel DeliveryChannels { get; set; } = DeliveryChannel.SignalR;
    public bool DeliveredViaSignalR { get; set; } = false;
    public bool DeliveredViaEmail { get; set; } = false;
    public bool DeliveredViaSMS { get; set; } = false;

    // == Optional delay support ==
    public DateTime? ScheduledAt { get; set; } // for delayed notifications
    public bool IsSent { get; set; } = false;

    // == Failure diagnostics (if you want to store it here) ==
    public string? LastErrorMessage { get; set; }
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
