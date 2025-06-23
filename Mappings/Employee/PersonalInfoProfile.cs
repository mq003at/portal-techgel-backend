using AutoMapper;
using portal.DTOs;
using portal.Models;

namespace portal.Mappings;

public class PersonalInfoProfile : Profile
{
    public PersonalInfoProfile()
    {
        // --------- Entity <-> Read DTO --------- //
        CreateMap<PersonalInfo, PersonalInfoDTO>().ReverseMap();

        // --------- Create DTO -> Entity --------- //
        CreateMap<CreatePersonalInfoDTO, PersonalInfo>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) // ensure EF generates ID
            .ForMember(dest => dest.EmployeeId, opt => opt.Ignore()) // assigned after Employee creation
            .ForMember(dest => dest.Employee, opt => opt.Ignore()); // avoid circular reference

        // --------- Update DTO -> Entity --------- //
        CreateMap<UpdatePersonalInfoDTO, PersonalInfo>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}