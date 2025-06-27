using System.Text.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using portal.Db;
using portal.DTOs;
using portal.Models;

namespace portal.Services;

public class NotificationService
    : BaseService<Notification, NotificationDTO, NotificationCreateDTO, NotificationUpdateDTO>,
      INotificationService
{
    public NotificationService(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<NotificationService> logger
    ) : base(context, mapper, logger)
    {
    }

    public async Task<IEnumerable<NotificationDTO>> GetByEmployeeIdAsync(int employeeId)
    {
        var notifications = await _dbSet
            .Where(n => n.EmployeeId == employeeId)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();

        return _mapper.Map<IEnumerable<NotificationDTO>>(notifications);
    }

    public async Task MarkAsReadAsync(int id)
    {
        var notification = await _dbSet.FindAsync(id);
        if (notification == null)
            throw new Exception($"Notification with ID {id} not found.");

        if (notification.ReadAt == null)
        {
            notification.ReadAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}
