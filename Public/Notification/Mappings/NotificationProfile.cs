using AutoMapper;
using portal.Models;
using portal.DTOs;
using portal.Extensions;

namespace portal.Mappings;
public class NotificationProfile : Profile
{
    public NotificationProfile()
    {
        // ----------- READ MAPPING ----------- //
        CreateMap<Notification, NotificationDTO>()
            .IncludeBase<BaseModel, BaseModelDTO>()
            .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee.GetDisplayName()))
            .ForMember(dest => dest.NotificationCategoryName, opt => opt.MapFrom(src =>
                src.NotificationCategory.Name));

        // ----------- CREATE MAPPING ----------- //
        CreateMap<NotificationCreateDTO, Notification>()
            .IncludeBase<BaseModelCreateDTO, BaseModel>();

        // ----------- UPDATE MAPPING ----------- //
        CreateMap<NotificationUpdateDTO, Notification>()
            .IncludeBase<BaseModelUpdateDTO, BaseModel>();

        // ----------- USER-SIDE PARTIAL UPDATE ----------- //
        CreateMap<NotificationUserUpdateDTO, Notification>()
            .IncludeBase<BaseModelUpdateDTO, BaseModel>();
    }
}
