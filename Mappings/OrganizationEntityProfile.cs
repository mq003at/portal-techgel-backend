namespace portal.Mappings;

using AutoMapper;
using portal.DTOs;
using portal.Enums;
using portal.Models;

public class OrganizationEntityProfile
    : BaseModelProfile<
        OrganizationEntity,
        OrganizationEntityDTO,
        CreateOrganizationEntityDTO,
        UpdateOrganizationEntityDTO
    >
{
    public OrganizationEntityProfile()
    {
        // Additional custom mapping (Model → DTO)
        CreateMap<OrganizationEntity, OrganizationEntityDTO>()
            .ForMember(
                dest => dest.ParentName,
                opt => opt.MapFrom(src => src.Parent != null ? src.Parent.Name : null)
            )
            .ForMember(
                dest => dest.ChildrenIds,
                opt =>
                    opt.MapFrom(src =>
                        src.Children != null
                            ? src.Children.Select(c => c.Id).ToList()
                            : new List<int>()
                    )
            )
            .ForMember(
                dest => dest.ChildrenNames,
                opt =>
                    opt.MapFrom(src =>
                        src.Children != null
                            ? src.Children.Select(c => c.Name).ToList()
                            : new List<string>()
                    )
            )
            .ForMember(
                dest => dest.EmployeeIds,
                opt => opt.MapFrom(src => src.Employees.Select(e => e.Id).ToList())
            )
            .ForMember(
                dest => dest.EmployeeMainIds,
                opt =>
                    opt.MapFrom(src => src.Employees.Select(e => e.MainId ?? string.Empty).ToList())
            )
            .ForMember(
                dest => dest.EmployeeFullNames,
                opt => opt.MapFrom(src => src.Employees.Select(e => FormatEmployeeName(e)).ToList())
            )
            .ForMember(
                dest => dest.EmployeeNames,
                opt => opt.MapFrom(src => src.Employees.Select(e => FormatEmployeeName(e)).ToList())
            )
            .ForMember(dest => dest.Employees, opt => opt.MapFrom(src => src.Employees))
            .ForMember(dest => dest.ManagerId, opt => opt.MapFrom(src => src.ManagerId))
            .ForMember(
                dest => dest.ManagerName,
                opt =>
                    opt.MapFrom(src =>
                        src.Manager != null ? FormatEmployeeName(src.Manager) : string.Empty
                    )
            )
            .ForMember(dest => dest.DeputyManagerId, opt => opt.MapFrom(src => src.DeputyManagerId))
            .ForMember(
                dest => dest.DeputyManagerName,
                opt =>
                    opt.MapFrom(src =>
                        src.DeputyManager != null ? FormatEmployeeName(src.DeputyManager) : null
                    )
            );

        // Optional: apply null-checking for update mapping
        CreateMap<UpdateOrganizationEntityDTO, OrganizationEntity>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
    }

    private static string FormatEmployeeName(Employee? e)
    {
        if (e == null)
            return string.Empty;
        return $"{e.LastName} {e.MiddleName} {e.FirstName}".Trim();
    }
}
