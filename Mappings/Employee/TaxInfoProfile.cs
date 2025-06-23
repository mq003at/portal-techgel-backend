using AutoMapper;
using portal.DTOs;
using portal.Models;

namespace portal.Mappings;
public class TaxInfoProfile : Profile
{
    public TaxInfoProfile()
    {
        // Entity ↔ Read DTO
        CreateMap<TaxInfo, TaxInfoDTO>().ReverseMap();

        // Create DTO → Entity
        CreateMap<CreateTaxInfoDTO, TaxInfo>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Employee, opt => opt.Ignore()); // prevent circular reference

        // Update DTO → Entity
        CreateMap<UpdateTaxInfoDTO, TaxInfo>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}