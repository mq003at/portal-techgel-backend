using AutoMapper;
using portal.Models;
using portal.DTOs;

namespace portal.Mappings;

public class NotificationCategoryProfile
    : BaseModelWithOnlyIdProfile<
        NotificationCategory,
        NotificationCategoryDTO,
        NotificationCategoryCreateDTO,
        NotificationCategoryUpdateDTO>
{
    public NotificationCategoryProfile()
    {
        // Map OnlyForOrganizationEntities → OrganizationEntityDTOs
        CreateMap<NotificationCategory, NotificationCategoryDTO>()
            .ForMember(dest => dest.OnlyForOrganizationEntities,
                opt => opt.MapFrom(src => src.OnlyForOrganizationEntities
                    .Select(x => x.OrganizationEntity)));

        // CreateDTO → NotificationCategory
        CreateMap<NotificationCategoryCreateDTO, NotificationCategory>()
            .ForMember(dest => dest.OnlyForOrganizationEntities, opt => opt.Ignore()); // set manually

        // UpdateDTO → NotificationCategory
        CreateMap<NotificationCategoryUpdateDTO, NotificationCategory>()
            .ForMember(dest => dest.OnlyForOrganizationEntities, opt => opt.Ignore()); // set manually
    }
}