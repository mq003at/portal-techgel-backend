using DotNetCore.CAP;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using portal.DTOs;
using portal.Models;
using portal.Services;

[Route("api/mock-notifications")]
[ApiController]
public class MockEventController : ControllerBase
{
    private readonly ICapPublisher _capPublisher;
    private readonly ILogger<MockEventController> _logger;

    public MockEventController(ICapPublisher capPublisher, ILogger<MockEventController> logger)
    {
        _capPublisher = capPublisher;
        _logger = logger;
    }

    [HttpPost("workflow-approved")]
    public async Task<IActionResult> SimulateWorkflowApproved(
        [FromBody] MockWorkflowApprovedEvent mock
    )
    {
        const string topic = "workflow.approved";

        _logger.LogInformation(
            "Mock publishing CAP event '{Topic}' with data: {@Data}",
            topic,
            mock
        );

        var approvalEvent = new CreateEvent
        {
            WorkflowId = "1",
            EmployeeId = mock.EmployeeId,
            ApproverName = mock.ApproverName,
            Status = mock.Status,
        };

        BackgroundJob.Enqueue<WorkflowEventHandler>(handler =>
            handler.HandleCreation(approvalEvent)
        );
        return Ok(new { message = $"Event '{topic}' published", data = mock });
    }
}

public class MockWorkflowApprovedEvent
{
    public int WorkflowId { get; set; }
    public int EmployeeId { get; set; }
    public string ApproverName { get; set; } = "Test Approver";
    public string Status { get; set; } = "APPROVED";
    public DateTime ApprovedAt { get; set; } = DateTime.UtcNow;
}
