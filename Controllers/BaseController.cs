namespace portal.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using portal.DTOs;
using portal.Models;
using portal.Services;

public abstract class BaseController<TModel, TDTO, TUpdateDTO> : ApiControllerBase
    where TModel : BaseModel
    where TDTO : BaseDTO<TModel>
    where TUpdateDTO : BaseDTO<TModel>
{
    protected readonly IBaseService<TModel, TDTO, TUpdateDTO> _service;

    public BaseController(IBaseService<TModel, TDTO, TUpdateDTO> service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TDTO>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TDTO>> GetById(int id)
    {
        var dto = await _service.GetByIdAsync(id);
        if (dto == null)
            return NotFound();
        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<TDTO>> Create([FromBody] TDTO dto)
    {
        var createdDto = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = createdDto.Id }, createdDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TDTO>> Update(int id, [FromBody] TUpdateDTO dto)
    {
        if (dto == null)
            return BadRequest(new { error = "Request body is null" });

        try
        {
            var updatedDto = await _service.UpdateAsync(id, dto);
            if (updatedDto == null)
                return NotFound(new { error = $"Employee with ID {id} not found" });

            return Ok(updatedDto);
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(400, new { error = "Invalid operation", details = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                new { error = "An unexpected error occurred", details = ex.Message }
            );
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success)
            return NotFound();
        return NoContent();
    }
}
