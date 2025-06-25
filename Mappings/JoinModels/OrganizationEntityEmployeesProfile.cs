namespace portal.Mappings;

using AutoMapper;
using portal.DTOs; 

public class OrganizationEntityEmployeeProfile : Profile
{
    public OrganizationEntityEmployeeProfile()
    {

        CreateMap<OrganizationEntityEmployee, OrganizationEntityEmployeeDTO>();

        CreateMap<CreateOrganizationEntityEmployeeDTO, OrganizationEntityEmployee>();

        CreateMap<UpdateOrganizationEntityEmployeeDTO, OrganizationEntityEmployee>()

            .ForMember(d => d.OrganizationEntityId, o => o.Ignore())
            .ForMember(d => d.EmployeeId, o => o.Ignore())

            .ForAllMembers(o => o.Condition((src, _, srcMember) => srcMember is not null));
    }
}