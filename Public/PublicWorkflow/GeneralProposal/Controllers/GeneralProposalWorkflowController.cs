using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using portal.DTOs;
using portal.Models;
using portal.Services;

namespace portal.Controllers;

[Route("api/general-proposal")]
public class GeneralProposalWorkflowController
    : BaseController<
        GeneralProposalWorkflow,
        GeneralProposalWorkflowDTO,
        GeneralProposalWorkflowCreateDTO,
        GeneralProposalWorkflowUpdateDTO
    >
{
    private readonly IGeneralProposalWorkflowService _workflowService;

    public GeneralProposalWorkflowController(IGeneralProposalWorkflowService workflowService)
        : base(workflowService)
    {
        _workflowService = workflowService;
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
    public override async Task<ActionResult<GeneralProposalWorkflowDTO>> Update(
        int id,
        [FromBody] GeneralProposalWorkflowUpdateDTO dto
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
    public override async Task<ActionResult<IEnumerable<GeneralProposalWorkflowDTO>>> GetAll()
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

        var targetIds = new List<int> { 3 };
        if (organizationIds.Any(targetIds.Contains))
            return Ok(await _workflowService.GetAllAsync());
        else
        {
            List<GeneralProposalWorkflowDTO> workflows =
                await _workflowService.GetAllByEmployeeIdAsync(id)
                ?? throw new Exception("Workflow not found.");
            return workflows;
        }
    }

    [HttpPost("{id}/generate-documents")]
    [Authorize]
    public async Task<IActionResult> GenerateDocuments(int id)
    {
        if (id < 0)
            return BadRequest("Invalid workflow ID.");

        var result = await _workflowService.FinalizeIfCompleteAsync(id);
        if (result == false)
            return NotFound("No documents generated for the provided workflow ID.");

        return Ok(true);
    }
}
