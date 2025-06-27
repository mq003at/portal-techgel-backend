namespace portal.Services;

using portal.DTOs;
using portal.Models;


public interface INotificationService : IBaseService<Notification, NotificationDTO, NotificationCreateDTO, NotificationUpdateDTO>
{
    Task<IEnumerable<NotificationDTO>> GetByEmployeeIdAsync(int employeeId);
    Task MarkAsReadAsync(int id);
}