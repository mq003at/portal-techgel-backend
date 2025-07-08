using AutoMapper;
using portal.DTOs;
using portal.Models;

namespace portal.Mappings;
public class InsuranceInfoProfile : Profile
{
    public InsuranceInfoProfile()
    {
        // Entity ↔ Read DTO (bi-directional)
        CreateMap<InsuranceInfo, InsuranceInfoDTO>().ReverseMap();

        // Create DTO → Entity
        CreateMap<InsuranceInfoCreateDTO, InsuranceInfo>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Employee, opt => opt.Ignore()); // avoid circular reference

        // Update DTO → Entity (only map non-null values)
        CreateMap<InsuranceInfoUpdateDTO, InsuranceInfo>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}