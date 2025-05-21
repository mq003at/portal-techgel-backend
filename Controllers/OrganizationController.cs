using Microsoft.AspNetCore.Mvc;
using portal.DTOs;
using portal.Models;
using portal.Services;

namespace portal.Controllers;

public class OrganizationEntitiesController
    : BaseController<
        OrganizationEntity,
        OrganizationEntitySummaryDTO,
        CreateOrganizationEntityDTO,
        UpdateOrganizationEntityDTO
    >
{
    public OrganizationEntitiesController(IOrganizationEntityService service)
        : base(service) { }

    [HttpPost]
    public override async Task<ActionResult<OrganizationEntitySummaryDTO>> Create(
        [FromBody] CreateOrganizationEntityDTO dto
    )
    {
        try
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public override async Task<ActionResult<OrganizationEntitySummaryDTO>> Update(
        int id,
        [FromBody] UpdateOrganizationEntityDTO dto
    )
    {
        try
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null)
                return NotFound();
            return Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
