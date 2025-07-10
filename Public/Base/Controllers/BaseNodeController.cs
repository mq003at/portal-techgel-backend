using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using portal.DTOs;
using portal.Models;
using portal.Services;

namespace portal.Controllers;

[ApiController]
[Route("api/[controller]s")]
public abstract class BaseNodeController<
    TNodeModel,
    TNodeDTO,
    TNodeCreateDTO,
    TNodeUpdateDTO,
    TWorkflow
>(IBaseNodeService<TNodeModel, TNodeDTO, TNodeCreateDTO, TNodeUpdateDTO, TWorkflow> nodeService)
    : ControllerBase
    where TNodeModel : BaseWorkflowNode
    where TNodeDTO : WorkflowNodeDTO
    where TNodeCreateDTO : WorkflowNodeCreateDTO
    where TNodeUpdateDTO : WorkflowNodeUpdateDTO
    where TWorkflow : BaseWorkflow
{
    private readonly IBaseNodeService<
        TNodeModel,
        TNodeDTO,
        TNodeCreateDTO,
        TNodeUpdateDTO,
        TWorkflow
    > _nodeService = nodeService;

    // PUT api/{nodes}/{id}/approve
    [HttpPut("{id}/approve")]
    public virtual async Task<IActionResult> Approve(int id, [FromBody] ApproveWithCommentDTO dto)
    {
        var success = await _nodeService.ApproveAsync(id, dto);
        return Ok(success);
    }

    // PUT api/{nodes}/{id}/reject
    [HttpPut("{id}/reject")]
    public virtual async Task<IActionResult> Reject(int id, [FromBody] RejectDTO dto)
    {
        var success = await _nodeService.RejectAsync(id, dto);
        return Ok(success);
    }
}
