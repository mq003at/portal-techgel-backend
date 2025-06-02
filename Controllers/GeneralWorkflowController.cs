using Microsoft.AspNetCore.Mvc;
using portal.DTOs;
using portal.Models;
using portal.Services;

namespace portal.Controllers;
[Route("api/[controller]s")]
public class GeneralWorkflowController
    : BaseController<
        GeneralWorkflow,
        GeneralWorkflowDTO,
        CreateGeneralWorkflowDTO,
        UpdateGeneralWorkflowDTO
    >
{
    private readonly IGeneralWorkflowService _workflowService;
    private readonly ILogger<GeneralWorkflowController> _logger;

    public GeneralWorkflowController(
        IGeneralWorkflowService workflowService,
        ILogger<GeneralWorkflowController> logger
    )
        : base(workflowService)
    {
        _workflowService = workflowService;
        _logger = logger;
    }

    // GET api/workflows/{id}/nodes
    [HttpGet("{id}/nodes")]
    public async Task<ActionResult<IEnumerable<ApprovalWorkflowNodeDTO>>> GetNodes(int id) =>
        Ok(await _workflowService.GetNodesAsync(id));

    // POST api/workflows/{id}/nodes
    [HttpPost("{id}/nodes")]
    public async Task<ActionResult<ApprovalWorkflowNodeDTO>> AddNode(
        int id,
        [FromBody] CreateApprovalWorkflowNodeDTO dto
    )
    {
        _logger.LogInformation("Adding node to workflow {WorkflowId}", id);
        var node = await _workflowService.AddNodeAsync(id, dto);
        return CreatedAtAction(nameof(GetNodes), new { id }, node);
    }

    // DELETE api/workflows/{id}/nodes/{nodeId}
    [HttpDelete("{id}/nodes/{nodeId}")]
    public async Task<IActionResult> RemoveNode(int id, int nodeId)
    {
        _logger.LogInformation("Removing node {NodeId} from workflow {WorkflowId}", nodeId, id);
        await _workflowService.RemoveNodeAsync(id, nodeId);
        return NoContent();
    }
}
