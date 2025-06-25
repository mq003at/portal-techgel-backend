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
        try
        {
            var created = await _entityService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                new { error = "An unexpected error occurred", details = ex.Message }
            );
        }
    }

    [HttpPut("{id}")]
    public override async Task<ActionResult<OrganizationEntityDTO>> Update(
        int id,
        [FromBody] UpdateOrganizationEntityDTO dto
    )
    {
        try
        {
            var updated = await _entityService.UpdateAsync(id, dto);
            if (updated == null)
                return NotFound(new { error = $"OrganizationEntity with ID {id} not found" });
            return Ok(updated);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                new { error = "An unexpected error occurred", details = ex.Message }
            );
        }
    }
}
