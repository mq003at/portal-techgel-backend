using Microsoft.AspNetCore.Mvc;
using portal.DTOs;
using portal.Models;
using portal.Services;

namespace portal.Controllers;

public class ApprovalWorkflowNodeController
    : BaseController<
        ApprovalWorkflowNode,
        ApprovalWorkflowNodeDTO,
        CreateApprovalWorkflowNodeDTO,
        UpdateApprovalWorkflowNodeDTO
    >
{
    private new readonly IApprovalWorkflowNodeService _service;
    private readonly ILogger<ApprovalWorkflowNodeController> _logger;

    public ApprovalWorkflowNodeController(
        IApprovalWorkflowNodeService service,
        ILogger<ApprovalWorkflowNodeController> logger
    )
        : base(service)
    {
        _service = service;
        _logger = logger;
    }

    // PUT: api/approvalworkflownodes/{id}/documents
    [HttpPut("{id}/documents")]
    public async Task<IActionResult> UpdateRelatedDocuments(
        int id,
        [FromBody] List<int> documentIds
    )
    {
        var result = await _service.UpdateRelatedDocument(documentIds, id);
        return Ok(result);
    }

    // POST: api/approvalworkflownodes/{nodeId}/sign/{documentId}
    [HttpPost("{nodeId}/sign/{documentId}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> SignDocument(
        int nodeId,
        int documentId,
        [FromForm] IFormFile file
    )
    {
        if (file == null)
            return BadRequest("File is required.");

        await using var stream = file.OpenReadStream();
        var fileName = file.FileName;

        var result = await _service.SignDocumentByUpdatingTheDocumentAsync(
            nodeId,
            documentId,
            stream,
            fileName
        );

        return Ok(result);
    }

    // GET: api/approvalworkflownodes/{id}/status
    [HttpGet("{id}/status")]
    public async Task<IActionResult> GetApprovalStatus(int id)
    {
        var status = await _service.CheckApprovalStatusAsync(id);
        return Ok(status);
    }

    // POST: api/approvalworkflownodes/{id}/supporting-documents
    [HttpPost("{id}/supporting-documents")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadSupportingDocuments(
        int id,
        [FromForm] List<IFormFile> files
    )
    {
        if (files == null || !files.Any())
            return BadRequest("No files uploaded.");

        await _service.UploadSupportingDocumentsAsync(id, files);
        return NoContent();
    }
}
