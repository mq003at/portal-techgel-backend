namespace portal.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static void MapHubs(this IEndpointRouteBuilder app)
    {
        app.MapHub<NotificationHub>("/hubs/notification").RequireAuthorization();

        // Add more hubs here if needed, for example, chat
        // app.MapHub<ChatHub>("/hubs/chat").RequireAuthorization();
    }
}
