using AutoMapper;
using portal.DTOs;
using portal.Models;

namespace portal.Mappings;
public class EmergencyContactInfoProfile : Profile
{
    public EmergencyContactInfoProfile()
    {
        // Entity ↔ Read DTO
        CreateMap<EmergencyContactInfo, EmergencyContactInfoDTO>().ReverseMap();

        // Create DTO → Entity
        CreateMap<CreateEmergencyContactInfoDTO, EmergencyContactInfo>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Employee, opt => opt.Ignore());

        // Update DTO → Entity (map only non-null fields)
        CreateMap<UpdateEmergencyContactInfoDTO, EmergencyContactInfo>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}