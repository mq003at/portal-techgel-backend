using Microsoft.AspNetCore.Mvc;
using portal.DTOs;
using portal.Services;

namespace portal.Controllers;

public class SignaturesController : ApiControllerBase
{
    private readonly ISignatureService _sigSvc;
    private readonly ILogger<SignaturesController> _logger;

    public SignaturesController(ISignatureService sigSvc, ILogger<SignaturesController> logger)
    {
        _sigSvc = sigSvc;
        _logger = logger;
    }

    // Single upload endpoint: creates or replaces in one go
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<SignatureDTO>> UploadOrReplace([FromForm] UploadSignatureDTO dto)
    {
        _logger.LogInformation(
            "Upload/Replace request for EmployeeId={EmployeeId}",
            dto.EmployeeId
        );

        // Choose path based on whether a signature exists
        var existing = await _sigSvc.GetByEmployeeAsync(dto.EmployeeId);
        SignatureDTO result;
        if (existing == null)
        {
            // No signature yet → create new
            result = await _sigSvc.UploadAsync(dto);
            return CreatedAtAction(
                nameof(GetMetadata),
                new { employeeId = result.EmployeeId },
                result
            );
        }
        else
        {
            // Already exists → replace
            result = await _sigSvc.UploadAndReplaceAsync(dto);
            return Ok(result);
        }
    }

    [HttpGet("employee/{employeeId}")]
    public async Task<ActionResult<SignatureDTO>> GetMetadata(int employeeId)
    {
        var dto = await _sigSvc.GetByEmployeeAsync(employeeId);
        if (dto == null)
            return NotFound();
        return Ok(dto);
    }

    [HttpDelete("employee/{employeeId}")]
    public async Task<IActionResult> Delete(int employeeId)
    {
        _logger.LogInformation("Delete request for EmployeeId={EmployeeId}", employeeId);
        await _sigSvc.DeleteSignatureAsync(employeeId);
        return NoContent();
    }

    [HttpGet("{employeeId}")]
    public async Task<IActionResult> GetStream(int employeeId)
    {
        try
        {
            var svg = await _sigSvc.GetSignatureStreamAsync(employeeId);
            return File(svg, "image/svg+xml");
        }
        catch (FileNotFoundException)
        {
            return NotFound();
        }
    }
}
