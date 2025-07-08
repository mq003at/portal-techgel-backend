using AutoMapper;
using portal.DTOs;
using portal.Models;

namespace portal.Mappings;
public class EmployeeQualificationInfoProfile : Profile
{
    public EmployeeQualificationInfoProfile()
    {
        // Entity ↔ Read DTO (bi-directional)
        CreateMap<EmployeeQualificationInfo, EmployeeQualificationInfoDTO>().ReverseMap();

        // Create DTO → Entity
        CreateMap<CreateEmployeeQualificationInfoDTO, EmployeeQualificationInfo>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Employee, opt => opt.Ignore()); // avoid circular reference

        // Update DTO → Entity (only map non-null values)
        CreateMap<UpdateEmployeeQualificationInfoDTO, EmployeeQualificationInfo>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}