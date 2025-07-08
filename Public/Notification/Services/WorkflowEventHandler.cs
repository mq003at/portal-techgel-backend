using DotNetCore.CAP;
using portal.Models;

public class WorkflowEventHandler
{
    private readonly INotificationCategoryResolver _resolver;

    public WorkflowEventHandler(INotificationCategoryResolver resolver)
    {
        _resolver = resolver;
    }

    [CapSubscribe("workflow.approved")]
    public async Task HandleApproval(ApprovalEvent evt)
    {
        await _resolver.ProcessEventAsync(evt, "workflow.approved");
    }

    [CapSubscribe("workflow.rejected")]
    public async Task HandleRejection(RejectEvent evt)
    {
        await _resolver.ProcessEventAsync(evt, "workflow.rejected");
    }
}
