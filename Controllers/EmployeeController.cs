namespace portal.Controllers;

using System.Security.Claims;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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

    [HttpGet]
    [Authorize]
    public override async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetAll()
    {
        // If user is HR
        string claimValue = User.FindFirst("OrganizationEntityIds")?.Value ?? throw new UnauthorizedAccessException("OrganizationEntityIds claim not found.");
        string idClaim = User.FindFirst("Id")?.Value ?? throw new UnauthorizedAccessException("Id claim not found.");
        int id = int.Parse(idClaim);
        var organizationIds = claimValue
            .Split(",", StringSplitOptions.RemoveEmptyEntries)
            .Select(id => int.Parse(id))
            .ToList();

        if (organizationIds.Contains(11)) return await base.GetAll();
        else
        {
            var employee = await _employeeService.GetByIdAsync(id);
            var employees = employee != null ? [employee] : new List<EmployeeDTO>();
            return employees;
        }
    }

    [HttpGet("me")]
    [Authorize]

    public IActionResult Me()
    {
        return Ok(new
        {
            IsAuthenticated = User.Identity?.IsAuthenticated,
            Name = User.Identity?.Name,
            Claims = User.Claims.Select(c => new { c.Type, c.Value })
        });
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
        
        // Get organizationEntityIds from OrganizationEntityEmployees table with employeeId=user.Id
        List<int> organizationEntityIds = user.OrganizationEntityEmployees
            .Select(oe => oe.OrganizationEntityId)
            .ToList();
        string organizationEntityIdsString = string.Join(",", organizationEntityIds);

        await AuthHelper.SignInEmployeeAsync(
            HttpContext,
            id: user.Id.ToString(),
            mainId: user.MainId ?? "",
            role: "Employee",
            organizationEntityIds: organizationEntityIds);

        return Ok(user);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("Cookies");
        return Ok(new { Message = "Logged out" });
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
