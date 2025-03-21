using AutoMapper;
using portal.DTOs;
using portal.Models;

public class SectionProfile : Profile
{
    public SectionProfile()
    {
        CreateMap<Section, SectionDTO>()
            .ForMember(dest => dest.UnitIds, opt => opt.MapFrom(src => src.Units.Select(u => u.Id)))
            .ReverseMap();

        CreateMap<UpdateSectionDTO, Section>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
