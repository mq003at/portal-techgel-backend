namespace portal.Mappings;

using AutoMapper;
using portal.DTOs;
using portal.Models;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<Employee, EmployeeDTO>()
            .ForMember(
                dest => dest.DivisionIds,
                opt => opt.MapFrom(src => src.EmployeeDivisions.Select(ed => ed.DivisionId))
            )
            .ForMember(
                dest => dest.DepartmentIds,
                opt => opt.MapFrom(src => src.EmployeeDepartments.Select(ed => ed.DepartmentId))
            )
            .ForMember(
                dest => dest.SectionIds,
                opt => opt.MapFrom(src => src.EmployeeSections.Select(es => es.SectionId))
            )
            .ForMember(
                dest => dest.UnitIds,
                opt => opt.MapFrom(src => src.EmployeeUnits.Select(eu => eu.UnitId))
            )
            .ForMember(
                dest => dest.TeamIds,
                opt => opt.MapFrom(src => src.EmployeeTeams.Select(et => et.TeamId))
            );
        ;
        CreateMap<EmployeeDTO, Employee>();

        CreateMap<CreateEmployeeDTO, Employee>();
    }
}
