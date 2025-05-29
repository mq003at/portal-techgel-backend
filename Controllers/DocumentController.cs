using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using portal.DTOs;
using portal.Services;

namespace portal.Controllers;

[ApiController]
[Route("api/[controller]s")]
public class DocumentController : ControllerBase
{
    private readonly IDocumentService _documentService;
    private readonly ILogger<DocumentController> _logger;

    public DocumentController(IDocumentService documentService, ILogger<DocumentController> logger)
    {
        _documentService = documentService;
        _logger = logger;
    }

    // GET: api/document
    [HttpGet]
    public async Task<IActionResult> GetAllMetaData()
    {
        var result = await _documentService.GetAllMetaDataAsync();
        return Ok(result);
    }

    // GET: api/document/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var dto = await _documentService.GetByIdAsync(id);
        if (dto is null)
            return NotFound();
        return Ok(dto);
    }

    // POST: api/document/metadata
    [HttpPost("metadata")]
    public async Task<IActionResult> CreateMetaData([FromBody] CreateDocumentDTO dto)
    {
        var created = await _documentService.CreateMetaDataAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // POST: api/document/upload
    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadDocument([FromForm] CreateDocumentDTO dto)
    {
        if (dto.File is null)
            return BadRequest("File is required.");

        var created = await _documentService.UploadDocumentAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT: api/document/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMetaData(
        [FromRoute] int id,
        [FromBody] UpdateDocumentDTO dto
    )
    {
        var updated = await _documentService.UpdateAsync(id, dto);
        return Ok(updated);
    }

    // PUT: api/document/{id}/upload
    [HttpPut("{id}/upload")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> ReplaceFile(
        [FromRoute] int id,
        [FromForm] UpdateDocumentDTO dto
    )
    {
        if (dto.File is null)
            return BadRequest("File is required.");

        var updated = await _documentService.UploadAndReplaceDocumentAsync(dto, id);
        return Ok(updated);
    }

    // DELETE: api/document/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var deleted = await _documentService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    // HEAD: api/document/exist?category=LEGAL&fileName=abc.svg
    [HttpHead("exist")]
    public async Task<IActionResult> CheckFileExist(
        [FromQuery] string category,
        [FromQuery] string fileName
    )
    {
        var exists = await _documentService.IsFileExistAsync(category, fileName);
        return exists ? Ok() : NotFound();
    }
}
