using System.CodeDom.Compiler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using portal.DTOs;
using portal.Models;
using portal.Services;

namespace portal.Controllers;

[Route("api/gate-pass")]
public class GatePassWorkflowController
    : BaseController<
        GatePassWorkflow,
        GatePassWorkflowDTO,
        GatePassWorkflowCreateDTO,
        GatePassWorkflowUpdateDTO
    >
{
    private readonly IGatePassWorkflowService _workflowService;

    public GatePassWorkflowController(IGatePassWorkflowService workflowService)
        : base(workflowService)
    {
        _workflowService = workflowService;
    }

    [HttpPost("{id}/generate-documents")]
    [Authorize]
    public async Task<IActionResult> GenerateDocuments(int id)
    {
        var result = await _workflowService.FinalizeIfCompleteAsync(id);
        if (result == false)
            return NotFound();
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public override async Task<ActionResult> Delete(int id)
    {
        if (!await _workflowService.DeleteWorkflowAsync(id))
            return NotFound();
        return NoContent();
    }

    [HttpPut("{id}")]
    [Authorize]
    public override async Task<ActionResult<GatePassWorkflowDTO>> Update(
        int id,
        [FromBody] GatePassWorkflowUpdateDTO dto
    )
    {
        if (dto == null)
            return BadRequest("Request body is null");

        bool updated = await _workflowService.UpdateWorkflowAsync(id, dto);
        if (updated == false)
            return NotFound();
        return Ok(updated);
    }

    // Tắt bớt các route nếu không cần → giữ nguyên GetAll, GetById, Create, Update, Delete
    [HttpGet("{id}/nodes")]
    public async Task<IActionResult> GetNodes(int id)
    {
        var nodes = await _workflowService.GetNodesByWorkflowIdAsync(id);
        return Ok(nodes);
    }

    [HttpGet]
    [Authorize]
    public override async Task<ActionResult<IEnumerable<GatePassWorkflowDTO>>> GetAll()
    {
        // If user is HR
        string claimValue =
            User.FindFirst("OrganizationEntityIds")?.Value
            ?? throw new UnauthorizedAccessException("OrganizationEntityIds claim not found.");
        string idClaim =
            User.FindFirst("Id")?.Value
            ?? throw new UnauthorizedAccessException("Id claim not found.");
        int id = int.Parse(idClaim);
        var organizationIds = claimValue
            .Split(",", StringSplitOptions.RemoveEmptyEntries)
            .Select(id => int.Parse(id))
            .ToList();

        var targetIds = new List<int> { 3, 10, 11, 12, 13, 64 };
        if (organizationIds.Any(targetIds.Contains))
            return await base.GetAll();
        else
        {
            List<GatePassWorkflowDTO> workflows =
                await _workflowService.GetAllByEmployeeIdAsync(id)
                ?? throw new Exception("Workflow not found.");
            return workflows;
        }
    }
}
