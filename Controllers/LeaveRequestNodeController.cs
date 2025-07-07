using Microsoft.AspNetCore.Mvc;
using portal.DTOs;
using portal.Models;
using portal.Services;

namespace portal.Controllers;

[ApiController]
[Route("api/leave-request-nodes")]
public class LeaveRequestNodeController
    : BaseController<
        LeaveRequestNode,
        LeaveRequestNodeDTO,
        LeaveRequestNodeCreateDTO,
        LeaveRequestNodeUpdateDTO
    >
{
    private readonly ILeaveRequestNodeService _nodeService;

    public LeaveRequestNodeController(ILeaveRequestNodeService nodeService)
        : base(nodeService)
    {
        _nodeService = nodeService;
    }

    // PUT api/leave-request-nodes/{id}/approve?approverId=123
    [HttpPut("{id}/approve")]
    public async Task<IActionResult> Approve(int id, [FromBody] ApproverDTO dto)
    {
        var success = await _nodeService.ApproveAsync(id, dto.ApproverId);
        return Ok(success);
    }

    // PUT api/leave-request-nodes/{id}/reject?approverId=123
    [HttpPut("{id}/reject")]
    public async Task<IActionResult> Reject(int id, [FromBody] RejectDTO dto)
    {
        var success = await _nodeService.RejectAsync(
            id,
            dto.ApproverId,
            dto.RejectReason
        );
        return Ok(success);
    }
}
