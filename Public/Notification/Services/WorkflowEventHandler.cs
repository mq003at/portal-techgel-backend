using System.Text.Json;
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
        _logger.LogError("WorkflowEventHandler CREATED");
        _logger.LogError("✅ WorkflowEventHandler constructed");
        Console.WriteLine("✅ WorkflowEventHandler constructed. Console logging enabled.");
    }

    [CapSubscribe("workflow.approved", Group = "cap.queue.portal-techgel-api.v1")]
    public async Task HandleApproval(ApprovalEvent evt)
    {
        _logger.LogError("➡ Received workflow.approved");
        await _resolver.ProcessEventAsync(evt, "workflow.approved");
    }

    [CapSubscribe("workflow.rejected", Group = "cap.queue.portal-techgel-api.v1")]
    public async Task HandleRejection(RejectEvent evt)
    {
        _logger.LogError("➡ Received workflow.rejected");
        await _resolver.ProcessEventAsync(evt, "workflow.rejected");
    }

    [CapSubscribe("leaverequest.workflow.created", Group = "cap.queue.portal-techgel-api.v1")]
    public async Task HandleCreation(CreateEvent evt)
    {
        Console.WriteLine("Handling workflow creation event: " + JsonSerializer.Serialize(evt));
        _logger.LogError("➡ Received leaverequest.workflow.created");
        await _resolver.ProcessEventAsync(evt, "leaverequest.workflow.created");
    }
}
