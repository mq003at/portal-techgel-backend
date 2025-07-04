using DocumentFormat.OpenXml.Math;
using portal.DTOs;

public class NotificationDTO : BaseModelDTO
{
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = null!;

    public long NotificationCategoryId { get; set; }
    public string NotificationCategoryName { get; set; } = null!;

    public string Title { get; set; } = null!;
    public string Message { get; set; } = null!;
    public string? Url { get; set; }
    public UrgencyLevel UrgencyLevel { get; set; }
    public DateTime? ReadAt { get; set; }
    public DateTime? FinishedAt { get; set; }
}

public class NotificationCreateDTO : BaseModelCreateDTO
{
    public int EmployeeId { get; set; }

    public long NotificationCategoryId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? Url { get; set; }
    public UrgencyLevel UrgencyLevel { get; set; } = UrgencyLevel.MEDIUM;
}

public class NotificationUpdateDTO : BaseModelUpdateDTO
{
    public long NotificationCategoryId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? Url { get; set; }
    public UrgencyLevel UrgencyLevel { get; set; } = UrgencyLevel.MEDIUM;
}

public class NotificationUserUpdateDTO : BaseModelUpdateDTO
{
    public DateTime? ReadAt { get; set; }
    public DateTime? FinishedAt { get; set; }
}
