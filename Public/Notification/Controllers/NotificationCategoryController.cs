using Microsoft.AspNetCore.Mvc;
using portal.DTOs;
using portal.Models;
using portal.Services;

namespace portal.Controllers;

[ApiController]
[Route("api/notificationcategories")]
public class NotificationCategoryController
    : BaseModelWithOnlyIdController<
        NotificationCategory,
        NotificationCategoryDTO,
        NotificationCategoryCreateDTO,
        NotificationCategoryUpdateDTO
    >
{
    private readonly INotificationCategoryService _notificationCategoryService;

    public NotificationCategoryController(INotificationCategoryService notificationCategoryService)
        : base(notificationCategoryService)
    {
        _notificationCategoryService = notificationCategoryService;
    }

    // Optional override if you want to expose explicit creation with validation later
    [HttpPost]
    public override async Task<ActionResult<NotificationCategoryDTO>> Create(
        [FromBody] NotificationCategoryCreateDTO dto
    )
    {
        var created = await _notificationCategoryService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public override async Task<ActionResult<NotificationCategoryDTO>> Update(
        int id,
        [FromBody] NotificationCategoryUpdateDTO dto
    )
    {
        var updated = await _notificationCategoryService.UpdateAsync(id, dto);
        if (updated == null)
            return NotFound();
        return Ok(updated);
    }
}
