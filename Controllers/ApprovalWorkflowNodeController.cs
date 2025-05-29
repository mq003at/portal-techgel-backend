using Microsoft.AspNetCore.Mvc;
using portal.DTOs;

namespace portal.Controllers;

[ApiController]
[Route("api/workflows/{workflowId}/nodes/{nodeId}/files")]
public class ApprovalWorkflowNodesFilesController : ControllerBase
{
    private readonly IApprovalWorkflowNodesFileService _fileService;
    private readonly ILogger<ApprovalWorkflowNodesFilesController> _logger;

    public ApprovalWorkflowNodesFilesController(
        IApprovalWorkflowNodesFileService fileService,
        ILogger<ApprovalWorkflowNodesFilesController> logger
    )
    {
        _fileService = fileService;
        _logger = logger;
    }

    /// <summary>
    /// Remove one or more files from an approval node.
    /// </summary>
    [HttpDelete]
    public async Task<IActionResult> DeleteFiles(
        [FromRoute] int workflowId,
        [FromRoute] int nodeId,
        [FromBody] DeleteFilesFromApprovalWorkflowNodesDTO dto
    )
    {
        _logger.LogInformation(
            "Deleting files {FileIds} from ApprovalWorkflowNodes {NodeId}",
            dto.FileIds,
            nodeId
        );
        await _fileService.DeleteFilesAsync(nodeId, dto.FileIds);
        return NoContent();
    }

    /// <summary>
    /// Upload new files and/or link existing documents to an approval node.
    /// </summary>
    [HttpPut]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateFiles(
        [FromRoute] int workflowId,
        [FromRoute] int nodeId,
        [FromForm] UpdateFilesInApprovalWorkflowNodesDTO dto
    )
    {
        _logger.LogInformation("Updating files for ApprovalWorkflowNodes {NodeId}", nodeId);

        var result = await _fileService.UpdateFilesAsync(
            nodeId,
            dto.NewFiles,
            dto.ExistingDocumentIds
        );

        return Ok(result);
    }
}
