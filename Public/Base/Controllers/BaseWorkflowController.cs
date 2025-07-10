using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using portal.DTOs;
using portal.Models;
using portal.Services;

namespace portal.Controllers;

[ApiController]
[Route("api/[controller]s")]
[Authorize]
public abstract class BaseWorkflowController<TModel, TReadDTO, TCreateDTO, TUpdateDTO, TNodeModel>
    : ControllerBase
    where TModel : BaseWorkflow
    where TReadDTO : BaseWorkflowDTO
    where TCreateDTO : BaseWorkflowCreateDTO
    where TUpdateDTO : BaseWorkflowUpdateDTO
    where TNodeModel : BaseWorkflowNode
{
    protected readonly IBaseWorkflowService<
        TModel,
        TReadDTO,
        TCreateDTO,
        TUpdateDTO,
        TNodeModel
    > _service;
    protected readonly IHttpContextAccessor _httpContextAccessor;

    protected BaseWorkflowController(
        IBaseWorkflowService<TModel, TReadDTO, TCreateDTO, TUpdateDTO, TNodeModel> service,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _service = service;
        _httpContextAccessor = httpContextAccessor;
    }

    protected int? CurrentUserId =>
        int.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirst("Id")?.Value, out var id)
            ? id
            : null;

    [HttpGet]
    public virtual async Task<ActionResult<IEnumerable<TReadDTO>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public virtual async Task<ActionResult<TReadDTO>> GetById(int id)
    {
        var dto = await _service.GetByIdAsync(id);
        return dto == null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public virtual async Task<ActionResult<TReadDTO>> Create([FromBody] TCreateDTO dto)
    {
        if (CurrentUserId is null)
            return Unauthorized();

        // Optionally inject CurrentUserId into dto if your workflow uses SenderId
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public virtual async Task<ActionResult<TReadDTO>> Update(int id, [FromBody] TUpdateDTO dto)
    {
        if (dto == null)
            return BadRequest("Invalid request");

        var updated = await _service.UpdateAsync(id, dto);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id}")]
    public virtual async Task<ActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        return result ? NoContent() : NotFound();
    }
}
