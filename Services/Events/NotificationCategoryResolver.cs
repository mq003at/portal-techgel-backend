using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text.Json;
using AutoMapper;
using DotNetCore.CAP;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using portal.Db;
using portal.Models;

public class NotificationCategoryResolver : INotificationCategoryResolver
{
    private readonly ApplicationDbContext _context;
    private readonly ICapPublisher _capPublisher;
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly ILogger<NotificationCategoryResolver> _logger;

    public NotificationCategoryResolver(
        ApplicationDbContext context,
        ICapPublisher capPublisher,
        IHubContext<NotificationHub> hubContext,
        ILogger<NotificationCategoryResolver> logger
    )
    {
        _context = context;
        _capPublisher = capPublisher;
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task ProcessEventAsync<TEvent>(TEvent evt, string triggerEvent)
    {
        var json = JsonSerializer.Serialize(evt);
        var matchedCategories = await _context
            .NotificationCategories.Where(c => c.TriggerEvent == triggerEvent)
            .ToListAsync();

        foreach (var category in matchedCategories)
        {
            // Step 1: Optional condition filtering
            if (!string.IsNullOrWhiteSpace(category.ConditionExpression))
            {
                var isMatch = EvaluateCondition(category.ConditionExpression, evt);
                if (!isMatch)
                {
                    _logger.LogInformation(
                        $"[Filtered] Condition not met: {category.ConditionExpression}"
                    );
                    continue;
                }
            }

            // Step 2: Extract employee ID
            var employeeId = ExtractEmployeeId(evt);
            if (employeeId == null)
            {
                _logger.LogWarning("Cannot resolve EmployeeId from event.");
                continue;
            }

            // Step 3: Delayed dispatch or immediate
            if (category.DelaySeconds.HasValue && category.DelaySeconds.Value > 0)
            {
                var delayEvent = new DelayedNotificationEvent
                {
                    NotificationCategoryId = category.Id,
                    EmployeeId = employeeId.Value,
                    EventPayload = json,
                    TriggerEvent = triggerEvent
                };

                await PublishDelayedNotificationAsync(
                    "notification.delayed",
                    delayEvent,
                    delayInMilliseconds: category.DelaySeconds.Value * 1000
                );
                continue;
            }

            await CreateNotificationNow(category, employeeId.Value, evt);
        }
    }

    private bool EvaluateCondition<T>(string condition, T data)
    {
        try
        {
            var properties =
                data?.GetType().GetProperties()
                ?? throw new ArgumentNullException(
                    nameof(data),
                    "Condition evaluation failed: data is null."
                );

            var dict = properties.ToDictionary(
                p => p.Name,
                p => (object?)p.GetValue(data) ?? DBNull.Value
            );

            var lambda = DynamicExpressionParser.ParseLambda<IDictionary<string, object>, bool>(
                new ParsingConfig(),
                false,
                condition
            );

            return lambda.Compile().Invoke(dict);
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Condition evaluation failed: {ex.Message}");
            return false;
        }
    }

    private int? ExtractEmployeeId<T>(T evt)
    {
        var prop = evt?.GetType().GetProperty("EmployeeId");
        if (prop == null)
            return null;
        return prop.GetValue(evt) as int? ?? Convert.ToInt32(prop.GetValue(evt));
    }

    private async Task CreateNotificationNow<T>(
        NotificationCategory category,
        int employeeId,
        T evt
    )
    {
        var title = RenderTemplate(category.TitleTemplate, evt);
        var message = RenderTemplate(category.MessageTemplate, evt);

        var notification = new Notification
        {
            EmployeeId = employeeId,
            NotificationCategoryId = category.Id,
            Title = title,
            Message = message,
            Url = null,
            UrgencyLevel = category.IsUrgentByDefault ? UrgencyLevel.HIGH : UrgencyLevel.MEDIUM,
            CreatedAt = DateTime.UtcNow
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();

        // Deliver via SignalR if required
        if (category.DeliveryChannels.HasFlag(DeliveryChannel.SignalR))
        {
            await _hubContext
                .Clients.User(employeeId.ToString())
                .SendAsync(
                    "new-notification",
                    new
                    {
                        notification.Id,
                        notification.Title,
                        notification.Message,
                        notification.CreatedAt,
                        notification.UrgencyLevel
                    }
                );
        }

        _logger.LogInformation(
            $"Notification delivered to Employee {employeeId} (Category: {category.Name})"
        );
    }

    private string RenderTemplate<T>(string template, T data)
    {
        var result = template;
        var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var prop in props)
        {
            var key = $"{{{prop.Name}}}";
            var value = prop.GetValue(data)?.ToString() ?? "";
            result = result.Replace(key, value, StringComparison.OrdinalIgnoreCase);
        }

        return result;
    }

    [CapSubscribe("notification.delayed")]
    public async Task HandleDelayedNotification(DelayedNotificationEvent evt)
    {
        var category = await _context.NotificationCategories.FirstOrDefaultAsync(c =>
            c.Id == evt.NotificationCategoryId
        );

        if (category == null)
        {
            _logger.LogWarning($"NotificationCategory {evt.NotificationCategoryId} not found.");
            return;
        }

        var type = ResolveEventTypeFromTrigger(evt.TriggerEvent);
        if (type == null)
        {
            _logger.LogWarning(
                $"Unrecognized TriggerEvent '{evt.TriggerEvent}' for delayed handler."
            );
            return;
        }

        var parsed = JsonSerializer.Deserialize(evt.EventPayload, type);
        if (parsed == null)
        {
            _logger.LogWarning($"Deserialization failed for TriggerEvent '{evt.TriggerEvent}'");
            return;
        }

        await CreateNotificationNow(category, evt.EmployeeId, parsed);
    }

    private static Type? ResolveEventTypeFromTrigger(string triggerEvent) =>
        triggerEvent switch
        {
            "workflow.approved" => typeof(ApprovalEvent),
            "workflow.rejected" => typeof(RejectEvent),
            // Add more here
            _ => null
        };

    public async Task PublishDelayedNotificationAsync<T>(
        string topic,
        T data,
        int delayInMilliseconds
    )
    {
        var headers = new Dictionary<string, string?>
        {
            ["x-delay"] = delayInMilliseconds.ToString()
        };

        await _capPublisher.PublishAsync(name: topic, contentObj: data, headers: headers);
    }
}
