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
        // RoleInfo → RoleInfoDTO
        CreateMap<RoleInfo, RoleInfoDTO>()
            .ForMember(d => d.SupervisorId, o => o.MapFrom(s => s.SupervisorId))
            .ForMember(
                d => d.SupervisorName,
                o =>
                    o.MapFrom(s =>
                        s.Supervisor != null
                            ? s.Supervisor.FirstName + " " + s.Supervisor.LastName
                            : null
                    )
            )
            .ForMember(d => d.SubordinateIds, o => o.MapFrom(s => s.Subordinates.Select(x => x.Id)))
            .ForMember(
                d => d.SubordinateNames,
                o => o.MapFrom(s => s.Subordinates.Select(x => x.FirstName + " " + x.LastName))
            )
            .ForMember(
                d => d.ManagedOrganizationEntityIds,
                o => o.MapFrom(s => s.ManagedOrganizationEntities.Select(e => e.Id))
            )
            .ForMember(
                d => d.ManagedOrganizationEntityNames,
                o => o.MapFrom(s => s.ManagedOrganizationEntities.Select(e => e.Name))
            )
            .ForMember(
                d => d.OrganizationEntityIds,
                o =>
                    o.MapFrom(s =>
                        s.OrganizationEntityEmployees.Select(oee => oee.OrganizationEntityId)
                    )
            )
            .ForMember(
                d => d.OrganizationEntityNames,
                o =>
                    o.MapFrom(s =>
                        s.OrganizationEntityEmployees.Select(oee => oee.OrganizationEntity.Name)
                    )
            )
            .ForMember(d => d.GroupId, o => o.MapFrom(s => s.GroupId));

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
            .ForMember(d => d.EmergencyContactInfo, o => o.MapFrom(s => s.EmergencyContactInfo))
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
