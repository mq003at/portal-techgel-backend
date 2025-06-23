using AutoMapper;
using portal.DTOs;
using portal.Models;

namespace portal.Mappings;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        // ---------- Entity <-> Read DTO ---------- //
        CreateMap<Employee, EmployeeDTO>()
            .ForMember(dest => dest.SupervisorName, opt => opt.MapFrom(src =>
                src.Supervisor != null ? $"{src.Supervisor.LastName} {src.Supervisor.MiddleName} {src.Supervisor.FirstName}" : null))
            .ForMember(dest => dest.DeputySupervisorName, opt => opt.MapFrom(src =>
                src.DeputySupervisor != null ? $"{src.DeputySupervisor.LastName} {src.DeputySupervisor.MiddleName} {src.DeputySupervisor.FirstName}" : null))
            .ForMember(dest => dest.SubordinateIds, opt => opt.MapFrom(src =>
                src.Subordinates.Select(e => e.Id)))
            .ForMember(dest => dest.SubordinateNames, opt => opt.MapFrom(src =>
                src.Subordinates.Select(e => $"{e.LastName} {e.MiddleName} {e.FirstName}")))
            .ForMember(dest => dest.DeputySubordinateIds, opt => opt.MapFrom(src =>
                src.DeputySubordinates.Select(e => e.Id)))
            .ForMember(dest => dest.DeputySubordinateNames, opt => opt.MapFrom(src =>
                src.DeputySubordinates.Select(e => $"{e.LastName} {e.MiddleName} {e.FirstName}")))
            .ForMember(dest => dest.OrganizationEntityIds, opt => opt.MapFrom(src =>
                src.OrganizationEntityEmployees.Select(o => o.OrganizationEntityId)))
            .ForMember(dest => dest.OrganizationEntityNames, opt => opt.Ignore()) // Set manually if needed
            .ReverseMap();
        CreateMap<PersonalInfo, PersonalInfoDTO>();
        CreateMap<CompanyInfo, CompanyInfoDTO>();
        CreateMap<CareerPathInfo, CareerPathInfoDTO>();
        CreateMap<TaxInfo, TaxInfoDTO>();
        CreateMap<InsuranceInfo, InsuranceInfoDTO>();
        CreateMap<ScheduleInfo, ScheduleInfoDTO>();
        CreateMap<Signature, Signature>();
        CreateMap<EmergencyContactInfo, EmergencyContactInfoDTO>();
        CreateMap<EmployeeQualificationInfo, EmployeeQualificationInfoDTO>();

        // ---------- Create DTO -> Entity ---------- //
        CreateMap<CreateEmployeeDTO, Employee>()
            .ForMember(dest => dest.PersonalInfo, opt => opt.MapFrom(src => src.PersonalInfo))
            .ForMember(dest => dest.SupervisorId, opt => opt.MapFrom(src => src.SupervisorId))
            .ForMember(dest => dest.DeputySupervisorId, opt => opt.MapFrom(src => src.DeputySupervisorId))
            .ForMember(dest => dest.Subordinates, opt => opt.Ignore())
            .ForMember(dest => dest.DeputySubordinates, opt => opt.Ignore())
            .ForMember(dest => dest.OrganizationEntityEmployees, opt => opt.Ignore());

        // ---------- Update DTO -> Entity ---------- //
        CreateMap<UpdateEmployeeDTO, Employee>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<PersonalInfoDTO, PersonalInfo>().ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<CompanyInfoDTO, CompanyInfo>().ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<CareerPathInfoDTO, CareerPathInfo>().ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<TaxInfoDTO, TaxInfo>().ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<InsuranceInfoDTO, InsuranceInfo>().ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<ScheduleInfoDTO, ScheduleInfo>().ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<Signature, Signature>().ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<UpdateEmployeeDetailsDTO, Employee>()
            .ForMember(dest => dest.CompanyInfo, opt => opt.Ignore()) // Handled manually (create/update pattern)
            .ForMember(dest => dest.ScheduleInfo, opt => opt.Ignore())
            .ForMember(dest => dest.CareerPathInfo, opt => opt.Ignore())
            .ForMember(dest => dest.TaxInfo, opt => opt.Ignore())
            .ForMember(dest => dest.InsuranceInfo, opt => opt.Ignore())
            .ForMember(dest => dest.EmergencyContactInfos, opt => opt.Ignore()) // Append mode
            .ForMember(dest => dest.EmployeeQualificationInfos, opt => opt.Ignore()) // Append mode
            .ForMember(dest => dest.SupervisorId, opt => opt.MapFrom(src => src.SupervisorId))
            .ForMember(dest => dest.DeputySupervisorId, opt => opt.MapFrom(src => src.DeputySubordinateIds != null ? (int?)null : null)) // handled manually
            .ForMember(dest => dest.Subordinates, opt => opt.Ignore()) // manual navigation fix
            .ForMember(dest => dest.DeputySubordinates, opt => opt.Ignore()) // same
            .ForMember(dest => dest.OrganizationEntityEmployees, opt => opt.Ignore()); // update manually
   }
}
