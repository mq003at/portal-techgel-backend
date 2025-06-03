using Microsoft.AspNetCore.Mvc;
using portal.DTOs;
using portal.Models;
using portal.Services;

namespace portal.Controllers;

[ApiController]
[Route("api/leave-request-nodes")]
public class LeaveRequestNodeController : BaseController<
    LeaveRequestNode,
    LeaveRequestNodeDTO,
    CreateLeaveRequestNodeDTO,
    UpdateLeaveRequestNodeDTO>
{
    private readonly ILeaveRequestNodeService _nodeService;

    public LeaveRequestNodeController(ILeaveRequestNodeService nodeService)
        : base(nodeService)
    {
        _nodeService = nodeService;
    }

    // PUT api/leave-request-nodes/{id}/approve?approverId=123
    [HttpPut("{id}/approve")]
    public async Task<IActionResult> Approve(int id, [FromQuery] int approverId, [FromQuery] string? comment = null)
    {
        var success = await _nodeService.ApproveAsync(id, approverId, comment);
        return success ? Ok("Node approved.") : BadRequest("Approval failed.");
    }
}