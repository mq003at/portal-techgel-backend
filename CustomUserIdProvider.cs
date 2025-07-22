using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

public class CustomUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        // Lấy từ claim "Id" hoặc "MainId"
        return connection.User?.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
    }
}
