namespace portal.Mappings;

using AutoMapper;
using portal.DTOs;
using portal.Enums;
using portal.Models;

public class OrganizationEntityProfile : Profile
{
    public OrganizationEntityProfile()
    {
        CreateMap<CreateOrganizationEntityDTO, OrganizationEntity>().ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) => srcMember != null)
            );
        CreateMap<UpdateOrganizationEntityDTO, OrganizationEntity>()
            // Bỏ qua Id và MainId nếu không muốn cập nhật
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) => srcMember != null)
            );
        // Lên mapping sang DTO summary hoặc full:
        CreateMap<OrganizationEntity, OrganizationEntityDTO>()
            .ForMember(
                dest => dest.ParentName,
                opt => opt.MapFrom(src => src.Parent != null ? src.Parent.Name : null)
            )
            .ForMember(
            dest => dest.ManagerId,
            opt => opt.MapFrom(src =>
                src.OrganizationEntityEmployees
                    .FirstOrDefault(oe => oe.OrganizationRelationType == OrganizationRelationType.MANAGER) != null
                    ? src.OrganizationEntityEmployees
                        .First(oe => oe.OrganizationRelationType == OrganizationRelationType.MANAGER).EmployeeId
                    : 0
            )
            )
            .ForMember(
                dest => dest.ManagerName,
                opt => opt.MapFrom(src =>
                    src.OrganizationEntityEmployees
                        .Where(oe => oe.OrganizationRelationType == OrganizationRelationType.MANAGER)
                        .Select(oe =>
                            oe.Employee.LastName+ " " + oe.Employee.MiddleName + " " + oe.Employee.FirstName)
                        .FirstOrDefault() ?? string.Empty
                )
            )
            .ForMember(
                dest => dest.DeputyManagerId,
                opt => opt.MapFrom(src =>
                    src.OrganizationEntityEmployees
                        .Where(oe => oe.OrganizationRelationType == OrganizationRelationType.DEPUTY_MANAGER)
                        .Select(oe => (int?)oe.EmployeeId)
                        .FirstOrDefault()
                )
            )
            .ForMember(
                dest => dest.DeputyManagerName,
                opt => opt.MapFrom(src =>
                    src.OrganizationEntityEmployees
                        .Where(oe => oe.OrganizationRelationType == OrganizationRelationType.DEPUTY_MANAGER)
                        .Select(oe =>
                            oe.Employee.LastName+ " " + oe.Employee.MiddleName + " " + oe.Employee.FirstName)
                        .FirstOrDefault()
                )
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
                dest => dest.EmployeeNames,
                opt =>
                    opt.MapFrom(src =>
                        src.OrganizationEntityEmployees.Select(oe =>
                            oe.Employee.FirstName
                            + " "
                            + oe.Employee.MiddleName
                            + " "
                            + oe.Employee.LastName
                        )
                            .ToList()
                    )
            )
        ;
    }
}
