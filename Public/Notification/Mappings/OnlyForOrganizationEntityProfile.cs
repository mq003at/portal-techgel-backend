using AutoMapper;
using portal.Models;
using portal.DTOs;
namespace portal.Mappings;

public class OnlyForOrganizationEntityProfile
    : BaseModelWithOnlyIdProfile<
        OnlyForOrganizationEntity,
        OnlyForOrganizationEntityDTO,
        OnlyForOrganizationEntityCreateDTO,
        OnlyForOrganizationEntityUpdateDTO>
{
    public OnlyForOrganizationEntityProfile()
    {
        CreateMap<OnlyForOrganizationEntity, OnlyForOrganizationEntityDTO>()
            .ForMember(dest => dest.NotificationCategoryName,
                opt => opt.MapFrom(src => src.NotificationCategory.Name))
            .ForMember(dest => dest.OrganizationEntityName,
                opt => opt.MapFrom(src => src.OrganizationEntity.Name));
    }
}
