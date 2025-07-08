using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using portal.DTOs;
using portal.Models;
using portal.Services;

namespace portal.Controllers;

[Route("api/[controller]s")]
[ApiController]
public class NotificationController
    : BaseController<Notification, NotificationDTO, NotificationCreateDTO, NotificationUpdateDTO>
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
        : base(notificationService)
    {
        _notificationService = notificationService;
    }

    // GET: api/notifications/employee/{employeeId}
    [HttpGet("employee/{employeeId}")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<NotificationDTO>>> GetByEmployeeId(int employeeId)
    {
        var result = await _notificationService.GetByEmployeeIdAsync(employeeId);
        return Ok(result);
    }

    // PUT: api/notifications/{id}/mark-as-read
    [HttpPut("{id}/mark-as-read")]
    [Authorize]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        try
        {
            await _notificationService.MarkAsReadAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
