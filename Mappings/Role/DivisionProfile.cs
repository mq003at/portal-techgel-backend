using AutoMapper;
using portal.DTOs;
using portal.Models;

public class DivisionProfile : Profile
{
    public DivisionProfile()
    {
        CreateMap<Division, DivisionDTO>()
            .ForMember(
                dest => dest.DepartmentIds,
                opt => opt.MapFrom(src => src.Departments.Select(d => d.Id))
            )
            .ReverseMap();
    }
}
