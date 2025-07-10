using DotNetCore.CAP;
using Microsoft.Extensions.Logging;
using portal.Models;

public class WorkflowEventHandler : ICapSubscribe
{
    private readonly INotificationCategoryResolver _resolver;
    private readonly ILogger<WorkflowEventHandler> _logger;

    public WorkflowEventHandler(
        INotificationCategoryResolver resolver,
        ILogger<WorkflowEventHandler> logger
    )
    {
        _resolver = resolver;
        _logger = logger;
        _logger.LogInformation("WorkflowEventHandler CREATED");
        _logger.LogInformation("✅ WorkflowEventHandler constructed");
    }

    [CapSubscribe("workflow.approved")]
    public async Task HandleApproval(ApprovalEvent evt)
    {
        _logger.LogInformation("➡ Received workflow.approved");
        await _resolver.ProcessEventAsync(evt, "workflow.approved");
    }

    [CapSubscribe("workflow.rejected")]
    public async Task HandleRejection(RejectEvent evt)
    {
        await _resolver.ProcessEventAsync(evt, "workflow.rejected");
    }
}
