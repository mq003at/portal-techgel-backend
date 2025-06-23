namespace portal.Controllers;

using Microsoft.AspNetCore.Mvc;
using portal.DTOs;
using portal.Models;
using portal.Services;

[Route("api/[controller]s")]
public class EmployeeController
    : BaseController<Employee, EmployeeDTO, CreateEmployeeDTO, UpdateEmployeeDTO>
{
    private readonly IEmployeeService _employeeService;
    private readonly ILogger<EmployeeController> _logger;

    public EmployeeController(IEmployeeService service, ILogger<EmployeeController> logger)
        : base(service)
    {
        _employeeService = service;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<ActionResult<EmployeeDTO>> Login([FromBody] LoginRequestDTO dto)
    {
        _logger.LogInformation("Login attempt for MainId={MainId}", dto.MainId);

        EmployeeDTO user;
        try
        {
            user = await _employeeService.LoginAsync(dto.MainId, dto.Password);
        }
        catch (UnauthorizedAccessException)
        {
            _logger.LogWarning("Invalid login for MainId={MainId}", dto.MainId);
            return Unauthorized("Invalid credentials");
        }

        _logger.LogInformation("Login successful for MainId={MainId}", dto.MainId);
        return Ok(user);
    }

    [HttpPut("{employeeId}/details")]
    public async Task<IActionResult> UpdateDetails(int employeeId, [FromBody] UpdateEmployeeDetailsDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var result = await _employeeService.UpdateEmployeeDetailsAsync(employeeId, dto);
            if (result == null)
                return NotFound($"Employee with ID {employeeId} not found.");

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating employee details for ID={EmployeeId}", employeeId);
            return StatusCode(500, "An error occurred while updating employee details.");
        }
    }
    }
