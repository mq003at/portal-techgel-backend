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
    // Retrieves all document metadata
    [HttpGet]
    public async Task<IActionResult> GetAllMetaData()
    {
        var result = await _documentService.GetAllMetaDataAsync();
        return Ok(result);
    }

    // GET: api/document/{id}
    // Retrieves document metadata by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var dto = await _documentService.GetMetaDataByIdAsync(id);
        if (dto is null)
            return NotFound();
        return Ok(dto);
    }

    // POST: api/document/metadata
    // Creates metadata for a document without uploading the file
    [HttpPost("metadata")]
    public async Task<IActionResult> CreateMetaData([FromBody] DocumentCreateDTO dto)
    {
        var created = await _documentService.CreateMetaDataAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // POST: api/document/upload
    // Uploads the document file and creates metadata
    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadDocument([FromForm] DocumentCreateDTO dto)
    {
        if (dto.File is null)
            return BadRequest("File is required.");

        var created = await _documentService.UploadDocumentAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT: api/document/{id}
    // Updates the document metadata - Use when only want to change filename or something else
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMetadata(
        [FromRoute] int id,
        [FromBody] DocumentUpdateDTO dto
    )
    {
        var updated = await _documentService.UpdateMetaDataAsync(id, dto);
        return Ok(updated);
    }

    [HttpPut("{id}/sign")]
    public async Task<IActionResult> SignFileAsync(int id, IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No signed file uploaded.");

        await using var stream = file.OpenReadStream();

        bool result;
        try
        {
            result = await _documentService.SignFileAsync(id, stream);
        }
        catch (FileNotFoundException ex)
        {
            return NotFound(ex.Message);
        }

        if (!result)
            return StatusCode(500, "Failed to replace file.");

        return Ok(new { success = true });
    }

    // Updates the document metadata and replaces the file
    // PUT: api/document/{id}/upload

    [HttpPut("{id}/upload")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> ReplaceFile(
        [FromRoute] int id,
        [FromForm] DocumentUpdateDTO dto
    )
    {
        if (dto.File is null)
            return BadRequest("File is required.");

        var updated = await _documentService.UploadAndReplaceDocumentAsync(dto, id);
        return Ok(updated);
    }

    // DELETE: api/document/{id}
    // Deletes the document metadata and file
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var deleted = await _documentService.DeleteMetaDataAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    // HEAD: api/document/exist?category=LEGAL&fileName=abc.png
    // Check if file exist in the specified category
    [HttpHead("exist")]
    public async Task<IActionResult> CheckFileExist(
        [FromQuery] string category,
        [FromQuery] string fileName
    )
    {
        var exists = await _documentService.IsFileExistAsync(category, fileName);
        return exists ? Ok() : NotFound();
    }

    // template handling
    // POST: api/document/template
    [HttpPost("template")]
    public async Task<ActionResult<DocumentDTO>> CreateTemplate(
        [FromForm] DocumentTemplateCreateDTO dto
    )
    {
        var result = await _documentService.CreateTemplateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }
}
