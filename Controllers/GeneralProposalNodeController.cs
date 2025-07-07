using Microsoft.AspNetCore.Mvc;
using portal.DTOs;
using portal.Models;
using portal.Services;

namespace portal.Controllers;

[ApiController]
[Route("api/general-proposal-nodes")]
public class GeneralProposalNodeController
    : BaseController<
        GeneralProposalNode,
        GeneralProposalNodeDTO,
        GeneralProposalNodeCreateDTO,
        GeneralProposalNodeUpdateDTO
    >
{
    private readonly IGeneralProposalNodeService _nodeService;

    public GeneralProposalNodeController(IGeneralProposalNodeService nodeService)
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
