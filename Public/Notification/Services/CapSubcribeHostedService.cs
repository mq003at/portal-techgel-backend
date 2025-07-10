public class CapSubscribeHostedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public CapSubscribeHostedService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // ✅ Create a scope to safely resolve scoped services
        using var scope = _serviceProvider.CreateScope();
        _ = scope.ServiceProvider.GetRequiredService<WorkflowEventHandler>();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
