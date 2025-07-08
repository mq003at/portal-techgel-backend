using portal.DTOs;
using portal.Models;

namespace portal.Services;

public interface INotificationCategoryService
    : IBaseModelWithOnlyIdService<
        NotificationCategory,
        NotificationCategoryDTO,
        NotificationCategoryCreateDTO,
        NotificationCategoryUpdateDTO
    >
{
    new Task<NotificationCategoryDTO> CreateAsync(NotificationCategoryCreateDTO dto);
    new Task<NotificationCategoryDTO?> UpdateAsync(int id, NotificationCategoryUpdateDTO dto);
}
