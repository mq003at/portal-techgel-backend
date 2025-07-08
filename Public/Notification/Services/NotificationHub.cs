using Microsoft.AspNetCore.SignalR;

public class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var employeeId = Context.User?.FindFirst("Id")?.Value;

        if (!string.IsNullOrEmpty(employeeId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user:{employeeId}");
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var employeeId = Context.User?.FindFirst("Id")?.Value;

        if (!string.IsNullOrEmpty(employeeId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user:{employeeId}");
        }

        await base.OnDisconnectedAsync(exception);
    }
}
