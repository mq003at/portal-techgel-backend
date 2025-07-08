public enum NotificationTargetType
{
    Employee = 0,
    Supervisor = 1,
    RoleBased = 2,
    CustomExpression = 3
}

[Flags]
public enum DeliveryChannel
{
    None = 0,
    SignalR = 1,
    Email = 2,
    SMS = 4,
    PushNotification = 8
}
