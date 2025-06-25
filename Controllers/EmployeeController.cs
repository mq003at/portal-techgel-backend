namespace portal.Controllers;

using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using portal.DTOs;
using portal.Models;
using portal.Services;
using portal.Services.JWT;

[Route("api/[controller]s")]
public class EmployeeController
    : BaseController<Employee, EmployeeDTO, CreateEmployeeDTO, UpdateEmployeeDTO>
{
    private readonly IEmployeeService _employeeService;
    private readonly ILogger<EmployeeController> _logger;
    private readonly IJwtService _jwtService;

    public EmployeeController(IEmployeeService service, ILogger<EmployeeController> logger, IJwtService jwtService)
        : base(service)
    {
        _employeeService = service;
        _logger = logger;
        _jwtService = jwtService;
    }

    [Authorize]
    [HttpGet()]
    public override async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetAll()
    {
        // If user is HR
        string claimValue = User.FindFirst("OrganizationEntityId")?.Value ?? throw new UnauthorizedAccessException("OrganizationEntityId claim not found.");
        string idClaim = User.FindFirst("Id")?.Value ?? throw new UnauthorizedAccessException("Id claim not found.");
        int id = int.Parse(idClaim);
        var organizationIds = claimValue
            .Split(",", StringSplitOptions.RemoveEmptyEntries)
            .Select(id => int.Parse(id))
            .ToList();

        if (organizationIds.Contains(11)) return await base.GetAll();
        else {
            var employee = await _employeeService.GetByIdAsync(id);
            var employees = employee != null ? [employee] : new List<EmployeeDTO>();
            return employees;
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginRequestDTO dto)
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

        string token = _jwtService.GenerateToken(
            user.Id.ToString(),
            user.MainId ?? "",
            "Employee",
            organizationEntityIds: user.OrganizationEntityIds
        );

        return Ok(new LoginResponseDTO
        {
            Token = token,
            Employee = user
        });
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
