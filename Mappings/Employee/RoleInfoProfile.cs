using AutoMapper;
using portal.DTOs;
using portal.Models;

namespace portal.Mappings;
public class RoleInfoProfile : Profile
{
    public RoleInfoProfile()
    {
        CreateMap<RoleInfoCreateDTO, RoleInfo>()
    .ForMember(dest => dest.Id, opt => opt.Ignore()) // in case you're using BaseModelWithOnlyId
    .ForMember(dest => dest.SupervisorId, opt => opt.MapFrom(src => src.SupervisorId))
    .ForMember(dest => dest.DeputySupervisorId, opt => opt.MapFrom(src => src.DeputySupervisorId))
    .ForMember(dest => dest.GroupId, opt => opt.MapFrom(src => src.GroupId))
    .ForMember(dest => dest.ManagedOrganizationEntities, opt => opt.Ignore())
    .ForMember(dest => dest.OrganizationEntityEmployees, opt => opt.Ignore())
    .ForMember(dest => dest.Subordinates, opt => opt.Ignore());

CreateMap<RoleInfoUpdateDTO, RoleInfo>()
    .ForMember(dest => dest.Id, opt => opt.Ignore())
    .ForMember(dest => dest.SupervisorId, opt => opt.MapFrom(src => src.SupervisorId))
    .ForMember(dest => dest.DeputySupervisorId, opt => opt.MapFrom(src => src.DeputySupervisorId))
    .ForMember(dest => dest.GroupId, opt => opt.MapFrom(src => src.GroupId))
    .ForMember(dest => dest.ManagedOrganizationEntities, opt => opt.Ignore())
    .ForMember(dest => dest.OrganizationEntityEmployees, opt => opt.Ignore())
    .ForMember(dest => dest.Subordinates, opt => opt.Ignore())
    .ForAllMembers(opt => opt.Condition((src, _, srcMember) => srcMember != null)); // for patching
    

        CreateMap<RoleInfo, RoleInfoDTO>()
            .ForMember(dest => dest.SupervisorName, opt => opt.MapFrom(src =>
                src.Supervisor != null
                    ? $"{src.Supervisor.LastName} {src.Supervisor.MiddleName} {src.Supervisor.FirstName}"
                    : null))

            .ForMember(dest => dest.DeputySupervisorName, opt => opt.MapFrom(src =>
                src.DeputySupervisor != null
                    ? $"{src.DeputySupervisor.LastName} {src.DeputySupervisor.MiddleName} {src.DeputySupervisor.FirstName}"
                    : null))

            .ForMember(dest => dest.SubordinateIds, opt => opt.MapFrom(src =>
                src.Subordinates.Select(e => e.Id).ToList()))

            .ForMember(dest => dest.SubordinateNames, opt => opt.MapFrom((src, dest, destMember, context) =>
            {
                var allEmployees = context.Items.TryGetValue("Employees", out var rawList)
                    && rawList is List<Employee> list
                    ? list
                    : new List<Employee>();

                return src.SubordinateIds
                    .Select(id =>
                    {
                        var emp = allEmployees.FirstOrDefault(e => e.Id == id);
                        return emp != null
                            ? $"{emp.LastName} {emp.MiddleName} {emp.FirstName}"
                            : null;
                    })
                    .Where(name => !string.IsNullOrWhiteSpace(name))
                    .ToList();
            }))

            .ForMember(dest => dest.ManagedOrganizationEntityIds, opt => opt.MapFrom(src =>
                src.ManagedOrganizationEntities.Select(e => e.Id).ToList()));
    }
}