namespace portal.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static void MapHubs(this IEndpointRouteBuilder app)
    {
        app.MapHub<NotificationHub>("/hubs/notification").RequireAuthorization();
        // app.MapHub<ChatHub>("/hubs/chat").RequireAuthorization();
    }
}
