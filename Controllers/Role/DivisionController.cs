namespace portal.Controllers;

using Microsoft.AspNetCore.Mvc;
using portal.DTOs;
using portal.Models;
using portal.Services;

public class DivisionController : BaseController<Division, DivisionDTO, UpdateDivisionDTO>
{
    private readonly IDivisionService _divisionService;

    public DivisionController(IDivisionService divisionService)
        : base(divisionService)
    {
        _divisionService = divisionService;
    }

    [HttpGet("full")]
    public async Task<ActionResult<IEnumerable<DivisionDTO>>> GetFullHierarchy()
    {
        var data = await _divisionService.GetFullHierarchyAsync();
        return Ok(data);
    }
}
