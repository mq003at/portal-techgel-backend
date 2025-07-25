using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
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
    private readonly List<string> _allowedExtensions =
        new() { ".docx", ".pdf", ".xlsx", ".png", ".svg", ".jpeg", ".jpg" };

    private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB

    public DocumentController(IDocumentService documentService, ILogger<DocumentController> logger)
    {
        _documentService = documentService;
        _logger = logger;
    }

    // Get the entire folder structure
    [HttpGet("folder-structure")]
    public async Task<IActionResult> GetFolderStructure([FromQuery] string? employeeMainId, [FromQuery] string? path)
    {
        var structure = string.IsNullOrWhiteSpace(employeeMainId)
        ? await _documentService.GetFolderStructure()
        : await _documentService.GetFolderStructure(employeeMainId, path);
        return Ok(structure);
    }

    // POST: upload 10 files at once, MAX 5MB each
    [HttpPost("upload-multiple")]
    [RequestSizeLimit(MaxFileSize * 10)] // Adjust if needed
    public async Task<ActionResult<List<DocumentDTO>>> UploadMultiple(
        [FromForm] DocumentUploadWrapperDTO dtos
    )
    {
        _logger.LogInformation(
            "Received multiple file upload request with {FileCount} files and raw metadata: {MetadataRaw}",
            dtos.Files.Count,
            dtos.Metadatas
        );

        List<DocumentCreateMetaDTO>? metaList;
        try
        {
            metaList = JsonSerializer.Deserialize<List<DocumentCreateMetaDTO>>(
                dtos.Metadatas,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter() }
                }
            );
            _logger.LogInformation(
                "Parsed metadata: {Metadata}",
                JsonSerializer.Serialize(metaList)
            );
        }
        catch (Exception ex)
        {
            return BadRequest("Invalid metadata format. " + ex.Message);
        }

        if (metaList == null || dtos.Files.Count != metaList.Count)
            return BadRequest("Mismatch between number of files and metadata entries.");

        var createDTOs = new List<DocumentCreateDTO>();

        for (int i = 0; i < dtos.Files.Count; i++)
        {
            var file = dtos.Files[i];
            var meta = metaList[i];

            if (file == null || file.Length == 0)
                return BadRequest($"File at index {i} is missing or empty.");

            if (file.Length > MaxFileSize)
                return BadRequest($"File {file.FileName} exceeds the 5MB size limit.");

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(ext))
                return BadRequest($"File {file.FileName} has an invalid extension.");

            _logger.LogInformation(
                "Processing file {FileName} with metadata: {Metadata}",
                file.FileName,
                JsonSerializer.Serialize(meta)
            );

            createDTOs.Add(
                new DocumentCreateDTO
                {
                    File = file,
                    Category = meta.Category,
                    Tag = meta.Tag,
                    Location = meta.Location,
                    Description = meta.Description
                }
            );
        }

        var result = await _documentService.MultipleUploadAsync(createDTOs);
        return Ok(result);
    }

    [HttpPost("download-multiple")]
    public async Task<IActionResult> DownloadMultiple([FromBody] List<string> fileUrls)
    {
        if (fileUrls == null || !fileUrls.Any())
            return BadRequest("No file URLs provided.");

        var fileDict = await _documentService.MultipleDownloadAsync(fileUrls);

        var zipStream = new MemoryStream();
        using (
            var archive = new System.IO.Compression.ZipArchive(
                zipStream,
                System.IO.Compression.ZipArchiveMode.Create,
                true
            )
        )
        {
            foreach (var (fileName, stream) in fileDict)
            {
                var entry = archive.CreateEntry(
                    fileName,
                    System.IO.Compression.CompressionLevel.Optimal
                );
                using var entryStream = entry.Open();
                stream.Position = 0;
                await stream.CopyToAsync(entryStream);
            }
        }

        var today = DateTime.UtcNow.ToString("yyyy-MM-dd");

        zipStream.Position = 0;
        return File(zipStream, "application/zip", $"documents_{today}.zip");
    }

    [HttpDelete("delete-multiple")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteMultipleFiles([FromBody] List<string> fileUrls)
    {
        if (fileUrls == null || !fileUrls.Any())
            return BadRequest("No file URLs provided.");

        try
        {
            var resultMessage = await _documentService.DeleteMultipleAsync(fileUrls);
            return Ok(new { message = resultMessage });
        }
        catch (FileNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (IOException ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while deleting files.");
            return StatusCode(500, new { error = "Unexpected error occurred." });
        }
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
