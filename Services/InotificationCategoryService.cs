using portal.DTOs;
using portal.Models;

namespace portal.Services;

public interface INotificationCategoryService
    : IBaseModelWithOnlyIdService<NotificationCategory, NotificationCategoryDTO, NotificationCategoryCreateDTO, NotificationCategoryUpdateDTO>
{
}