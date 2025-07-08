using Microsoft.AspNetCore.Mvc;
using portal.DTOs;
using portal.Models;
using portal.Services;

namespace portal.Controllers;

[ApiController]
[Route("api/gate-pass-nodes")]
public class GatePassNodeController
    : BaseController<GatePassNode, GatePassNodeDTO, GatePassNodeCreateDTO, GatePassNodeUpdateDTO>
{
    private readonly IGatePassNodeService _nodeService;

    public GatePassNodeController(IGatePassNodeService nodeService)
        : base(nodeService)
    {
        _nodeService = nodeService;
    }

    [HttpPut("{id}/approve")]
    public async Task<IActionResult> Approve(int id, [FromBody] ApproveWithCommentDTO dto)
    {
        var success = await _nodeService.ApproveAsync(id, dto);
        return Ok(success);
    }

    [HttpPut("{id}/reject")]
    public async Task<IActionResult> Reject(int id, [FromBody] RejectDTO dto)
    {
        var success = await _nodeService.RejectAsync(id, dto);
        return Ok(success);
    }
}
