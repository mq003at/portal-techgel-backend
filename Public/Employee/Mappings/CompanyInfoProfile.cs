using AutoMapper;
using portal.DTOs;
using portal.Models;

namespace portal.Mappings;
public class CompanyInfoProfile : Profile
{
    public CompanyInfoProfile()
    {
        // ---------- Entity <-> Read DTO ---------- //
        CreateMap<CompanyInfo, CompanyInfoDTO>().ReverseMap();

        // ---------- Create DTO -> Entity ---------- //
        CreateMap<CompanyInfoCreateDTO, CompanyInfo>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Employee, opt => opt.Ignore());

        // ---------- Update DTO -> Entity ---------- //
        CreateMap<CompanyInfoUpdateDTO, CompanyInfo>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}