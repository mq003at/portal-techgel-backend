using AutoMapper;
using portal.Models;
using portal.DTOs;

public class OrganizationEntityEmployeeProfile : Profile
{
    public OrganizationEntityEmployeeProfile()
    {
        CreateMap<OrganizationEntityEmployee, OrganizationEntityEmployeeDTO>()
            .ForMember(dest => dest.EmployeeName,
                   opt => opt.MapFrom(src => src.Employee != null
                   ? string.Join(" ",
                       new[] {
                       src.Employee.LastName,
                       src.Employee.MiddleName,
                       src.Employee.FirstName
                       }.Where(n => !string.IsNullOrWhiteSpace(n)))
                   : null))
            .ForMember(dest => dest.OrganizationEntityName,
                   opt => opt.MapFrom(src => src.OrganizationEntity != null ? src.OrganizationEntity.Name : null));

        CreateMap<OrganizationEntityEmployeeDTO, OrganizationEntityEmployee>();

        CreateMap<CreateOrganizationEntityEmployeeDTO, OrganizationEntityEmployee>();

        CreateMap<UpdateOrganizationEntityEmployeeDTO, OrganizationEntityEmployee>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}