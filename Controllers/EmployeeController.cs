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
}
