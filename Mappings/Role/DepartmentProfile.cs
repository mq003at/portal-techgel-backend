using AutoMapper;
using portal.DTOs;
using portal.Models;

public class DepartmentProfile : Profile
{
    public DepartmentProfile()
    {
        CreateMap<Department, DepartmentDTO>()
            .ForMember(
                dest => dest.SectionIds,
                opt => opt.MapFrom(src => src.Sections.Select(s => s.Id))
            )
            .ReverseMap();

        CreateMap<UpdateDepartmentDTO, Department>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
