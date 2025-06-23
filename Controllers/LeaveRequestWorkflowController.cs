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
    LeaveRequestWorkflowCreateDTO,
    LeaveRequestWorkflowUpdateDTO>
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

    // replace get with this one. If else for me. Check OrganizationEntityEmployees to see if this employeeId has access to organizationEntityId = 11

}