using Microsoft.AspNetCore.Mvc;
using portal.DTOs;
using portal.Models;
using portal.Services;

namespace portal.Controllers;

public class OrganizationEntitiesController
    : BaseController<
        OrganizationEntity,
        OrganizationEntityDTO,
        CreateOrganizationEntityDTO,
        UpdateOrganizationEntityDTO
    >
{
    private readonly IOrganizationEntityService _entityService;

    public OrganizationEntitiesController(IOrganizationEntityService service)
        : base(service)
    {
        _entityService = service;
    }

    [HttpPost]
    public override async Task<ActionResult<OrganizationEntityDTO>> Create(
        [FromBody] CreateOrganizationEntityDTO dto
    )
    {
        
        OrganizationEntityDTO created = await _entityService.CreateAsync(dto) ?? throw new InvalidOperationException("Failed to create organization entity.");
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);

    }

    [HttpPut("{id}")]
    public override async Task<ActionResult<OrganizationEntityDTO>> Update(
        int id,
        [FromBody] UpdateOrganizationEntityDTO dto
    )
    {
        return await base.Update(id, dto);
    }

    // Change who belongs to what orgentities
    // [HttpPut("{id}/employees")]
    // public async Task<IActionResult> UpdateEmployees(
    //     int id,
    //     [FromBody]  dto
    // )
    // {
    //     var result = await _entityService.UpdateEmployeesAsync(id, dto);
    //     return Ok(result);
    // }
}
