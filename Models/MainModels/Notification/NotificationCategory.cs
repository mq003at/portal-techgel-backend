namespace portal.Models;

public class NotificationCategory : BaseModelWithOnlyId
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public bool IsUrgentByDefault { get; set; } = false;
    public List<Notification> Notifications { get; set; } = new List<Notification>();
    public List<OnlyForOrganizationEntity> OnlyForOrganizationEntities { get; set; } =
        new List<OnlyForOrganizationEntity>();

    // CAP configuration
    // LeaveRequest.created
    public string TriggerEvent { get; set; } = null!;

    public string TitleTemplate { get; set; } = "Thông báo: {Action} {Entity}";
    public string MessageTemplate { get; set; } = "{Actor} đã {Action} từ {Entity}";

    // Target what ID
    public NotificationTargetType TargetType { get; set; } = NotificationTargetType.Employee;

    // Example: who receives this notification (e.g., Actor, TargetUser, Supervisor)
    public string TargetExpression { get; set; } = null!;

    // Time delayed before sent
    public int? DelaySeconds { get; set; }

    // Where to deliver
    public DeliveryChannel DeliveryChannels { get; set; } = DeliveryChannel.SignalR;
    public string? ConditionExpression { get; set; }
}
