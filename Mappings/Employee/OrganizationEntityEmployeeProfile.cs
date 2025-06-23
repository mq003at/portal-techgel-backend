using AutoMapper;
using portal.Models;
using portal.DTOs;

public class OrganizationEntityEmployeeProfile : Profile
{
    public OrganizationEntityEmployeeProfile()
    {
        CreateMap<OrganizationEntityEmployee, OrganizationEntityEmployeeDTO>()
            .ReverseMap();

        CreateMap<CreateOrganizationEntityEmployeeDTO, OrganizationEntityEmployee>();

        CreateMap<UpdateOrganizationEntityEmployeeDTO, OrganizationEntityEmployee>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}