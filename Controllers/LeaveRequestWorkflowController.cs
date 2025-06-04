using Microsoft.AspNetCore.Mvc;
using portal.DTOs;
using portal.Models;
using portal.Services;

namespace portal.Controllers;

[ApiController]
[Route("api/leave-requests")]
public class LeaveRequestWorkflowController : BaseController<
    LeaveRequestWorkflow,
    LeaveRequestWorkflowDTO,
    CreateLeaveRequestWorkflowDTO,
    UpdateLeaveRequestWorkflowDTO>
{
    private readonly ILeaveRequestWorkflowService _workflowService;

    public LeaveRequestWorkflowController(ILeaveRequestWorkflowService workflowService)
        : base(workflowService)
    {
        _workflowService = workflowService;
    }

    // Tắt bớt các route nếu không cần → giữ nguyên GetAll, GetById, Create, Update, Delete


    // GET api/leave-requests/{id}/nodes
    [HttpGet("{id}/nodes")]
    public async Task<IActionResult> GetNodes(int id)
    {
        var nodes = await _workflowService.GetNodesByWorkflowIdAsync(id);
        return Ok(nodes);
    }

    // PUT api/leave-requests/{id}/finalize
    [HttpPut("{id}/finalize")]
    public async Task<IActionResult> FinalizeWorkflow(int id)
    {
        var finalized = await _workflowService.FinalizeIfCompleteAsync(id);
        return finalized ? Ok("Workflow finalized") : BadRequest("Workflow not yet complete.");
    }
}
