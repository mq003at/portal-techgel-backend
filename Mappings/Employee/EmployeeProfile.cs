using AutoMapper;
using portal.DTOs;
using portal.Models;

namespace portal.Mappings;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        // Nested owned type mappings (Model <-> DTO)
        CreateMap<PersonalInfo, PersonalInfoDTO>()
            .ReverseMap();
        CreateMap<CompanyInfo, CompanyInfoDTO>().ReverseMap();
        CreateMap<CareerPathInfo, CareerPathInfoDTO>().ReverseMap();
        CreateMap<TaxInfo, TaxInfoDTO>().ReverseMap();
        CreateMap<InsuranceInfo, InsuranceInfoDTO>().ReverseMap();
        CreateMap<EmergencyContactInfo, EmergencyContactInfoDTO>().ReverseMap();
        CreateMap<ScheduleInfo, ScheduleInfoDTO>().ReverseMap();
        CreateMap<Signature, SignatureDTO>().ReverseMap();
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

        .ForMember(dest => dest.SubordinateNames, opt => opt.MapFrom((src, _, __, context) =>
        {
            var employees = context.Items.TryGetValue("Employees", out var rawList) && rawList is List<Employee> list
                ? list
                : new List<Employee>();

            return src.SubordinateIds
                .Select(id => employees.FirstOrDefault(e => e.Id == id))
                .Where(e => e != null)
                .Select(e => $"{e.LastName} {e.MiddleName} {e.FirstName}")
                .ToList();
        }))

        .ForMember(dest => dest.ManagedOrganizationEntityIds, opt => opt.MapFrom(src =>
            src.ManagedOrganizationEntities.Select(e => e.Id).ToList()))

        .ForMember(dest => dest.ManagedOrganizationEntityNames, opt => opt.MapFrom(src =>
            src.ManagedOrganizationEntities.Select(e => e.Name).ToList()))

        .ForMember(dest => dest.OrganizationEntityIds, opt => opt.MapFrom(src =>
            src.OrganizationEntityEmployees.Select(e => e.OrganizationEntityId).ToList()))

        .ForMember(dest => dest.OrganizationEntityNames, opt => opt.MapFrom(src =>
            src.OrganizationEntityEmployees.Select(e => e.OrganizationEntity != null ? e.OrganizationEntity.Name : "").ToList()));
            
        // RoleInfo → RoleInfoDTO


        // Employee → EmployeeDTO
        CreateMap<Employee, EmployeeDTO>()
            // scalar props
            .ForMember(d => d.FirstName, o => o.MapFrom(s => s.FirstName))
            .ForMember(d => d.MiddleName, o => o.MapFrom(s => s.MiddleName))
            .ForMember(d => d.LastName, o => o.MapFrom(s => s.LastName))
            .ForMember(d => d.Avatar, o => o.MapFrom(s => s.Avatar))
            .ForMember(d => d.Password, o => o.MapFrom(s => s.Password))
            .ForMember(d => d.MainId, o => o.MapFrom(s => s.MainId))
            // owned types
            .ForMember(d => d.PersonalInfo, o => o.MapFrom(s => s.PersonalInfo))
            .ForMember(d => d.CompanyInfo, o => o.MapFrom(s => s.CompanyInfo))
            .ForMember(d => d.CareerPathInfo, o => o.MapFrom(s => s.CareerPathInfo))
            .ForMember(d => d.TaxInfo, o => o.MapFrom(s => s.TaxInfo))
            .ForMember(d => d.InsuranceInfo, o => o.MapFrom(s => s.InsuranceInfo))
            .ForMember(d => d.EmergencyContactInfos, o => o.MapFrom(s => s.EmergencyContactInfos))
            .ForMember(d => d.ScheduleInfo, o => o.MapFrom(s => s.ScheduleInfo))
            .ForMember(d => d.Signature, o => o.MapFrom(s => s.Signature))
            // nested RoleInfoDTO
            .ForMember(d => d.RoleInfo, o => o.MapFrom(s => s.RoleInfo));

        // CreateEmployeeDTO → Employee
        CreateMap<CreateEmployeeDTO, Employee>()
            .ForMember(dest => dest.MainId, o => o.MapFrom(src => src.MainId))
            .ForMember(dest => dest.Id, o => o.Ignore())
            .ForMember(d => d.RoleInfo, o => o.Ignore())
            .ForMember(d => d.Signature, o => o.Ignore());

        // UpdateEmployeeDTO → Employee (only non-null src members)
        CreateMap<UpdateEmployeeDTO, Employee>()
            .ForMember(dest => dest.MainId, o => o.MapFrom(src => src.MainId))
            .ForMember(dest => dest.Id, o => o.Ignore())
            .ForMember(d => d.RoleInfo, o => o.Ignore())
            .ForMember(d => d.Signature, o => o.Ignore())
            .ForAllMembers(o => o.Condition((src, _, srcMember) => srcMember != null));
    }
}
