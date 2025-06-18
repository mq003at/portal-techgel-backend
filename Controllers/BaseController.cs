// portal/Controllers/BaseController.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using portal.DTOs;
using portal.Models;
using portal.Services;

namespace portal.Controllers;

public abstract class BaseController<TModel, TReadDTO, TCreateDTO, TUpdateDTO> : ApiControllerBase
    where TModel : BaseModel
    where TReadDTO : BaseModelDTO
    where TCreateDTO : BaseModelCreateDTO
    where TUpdateDTO : BaseModelUpdateDTO
{
    protected readonly IBaseService<TModel, TReadDTO, TCreateDTO, TUpdateDTO> _service;

    public BaseController(IBaseService<TModel, TReadDTO, TCreateDTO, TUpdateDTO> service)
    {
        _service = service;
    }

    [HttpGet]
    public virtual async Task<ActionResult<IEnumerable<TReadDTO>>> GetAll() =>
        Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public virtual async Task<ActionResult<TReadDTO>> GetById(int id)
    {
        var dto = await _service.GetByIdAsync(id);
        if (dto == null)
            return NotFound();
        return Ok(dto);
    }

    [HttpPost]
    public virtual async Task<ActionResult<TReadDTO>> Create([FromBody] TCreateDTO dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public virtual async Task<ActionResult<TReadDTO>> Update(int id, [FromBody] TUpdateDTO dto)
    {
        if (dto == null)
            return BadRequest("Request body is null");

        var updated = await _service.UpdateAsync(id, dto);
        if (updated == null)
            return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public virtual async Task<ActionResult> Delete(int id)
    {
        if (!await _service.DeleteAsync(id))
            return NotFound();
        return NoContent();
    }
}
