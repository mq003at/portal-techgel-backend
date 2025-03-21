using AutoMapper;
using portal.DTOs;
using portal.Models;

public class UnitProfile : Profile
{
    public UnitProfile()
    {
        CreateMap<Unit, UnitDTO>()
            .ForMember(dest => dest.TeamIds, opt => opt.MapFrom(src => src.Teams.Select(t => t.Id)))
            .ReverseMap();

        CreateMap<UpdateUnitDTO, Unit>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
