using AutoMapper;
using portal.DTO;
using portal.DTOs;
using portal.Models;

namespace portal.DTO.Profiles;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        // Model → ReadDTO (EmployeeDTO)
        CreateMap<Employee, EmployeeDTO>()
            .ForMember(dest => dest.PersonalInfo, opt => opt.MapFrom(src => src.PersonalInfo))
            .ForMember(dest => dest.CompanyInfo, opt => opt.MapFrom(src => src.CompanyInfo))
            .ForMember(dest => dest.CareerPathInfo, opt => opt.MapFrom(src => src.CareerPathInfo))
            .ForMember(dest => dest.TaxInfo, opt => opt.MapFrom(src => src.TaxInfo))
            .ForMember(dest => dest.InsuranceInfo, opt => opt.MapFrom(src => src.InsuranceInfo))
            .ForMember(
                dest => dest.EmergencyContactInfo,
                opt => opt.MapFrom(src => src.EmergencyContactInfo)
            )
            .ForMember(dest => dest.ScheduleInfo, opt => opt.MapFrom(src => src.ScheduleInfo))
            .ForMember(dest => dest.RoleInfo, opt => opt.MapFrom(src => src.RoleDetails))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.MainID, opt => opt.MapFrom(src => src.MainID));

        // Owned types → DTO
        CreateMap<PersonalInfo, PersonalInfoDTO>();
        CreateMap<CompanyInfo, CompanyInfoDTO>();
        CreateMap<CareerPathInfo, CareerPathInfoDTO>()
            .ForMember(d => d.Degree, o => o.MapFrom(s => s.Degrees))
            .ForMember(d => d.Certification, o => o.MapFrom(s => s.Certifications))
            .ForMember(d => d.Specialization, o => o.MapFrom(s => s.Specializations));
        CreateMap<TaxInfo, TaxInfoDTO>();
        CreateMap<InsuranceInfo, InsuranceInfoDTO>();
        CreateMap<EmergencyContactInfo, EmergencyContactInfoDTO>()
            .ForMember(d => d.EmergencyContactName, o => o.MapFrom(s => s.Name))
            .ForMember(d => d.EmergencyContactPhone, o => o.MapFrom(s => s.Phone))
            .ForMember(d => d.Relationship, o => o.MapFrom(s => s.Relationship))
            .ForMember(d => d.EmergencyContactCurrentAddress, o => o.MapFrom(s => s.CurrentAddress))
            .ForMember(
                d => d.EmergencyContactPermanentAddress,
                o => o.MapFrom(s => s.PermanentAddress)
            );
        CreateMap<ScheduleInfo, ScheduleInfoDTO>();

        // RoleDetails → RoleInfoDTO
        CreateMap<IEnumerable<EmployeeRoleDetail>, RoleInfoDTO>()
            .ConstructUsing(
                (src, ctx) =>
                    new RoleInfoDTO
                    {
                        RoleDetailsInfo = ctx.Mapper.Map<List<RoleDetailsInfoDTO>>(src)
                    }
            );

        CreateMap<EmployeeRoleDetail, RoleDetailsInfoDTO>()
            .ForMember(d => d.OrganizationEntityId, o => o.MapFrom(s => s.OrganizationEntityId))
            .ForMember(
                d => d.OrganizationEntityName,
                o => o.MapFrom(s => s.OrganizationEntity.Name)
            )
            .ForMember(
                d => d.ManagesOrganizationEntityId,
                o => o.MapFrom(s => s.ManagesOrganizationEntityId)
            )
            .ForMember(
                d => d.ManagesOrganizationEntityName,
                o =>
                    o.MapFrom(s =>
                        s.ManagesOrganizationEntity != null
                            ? s.ManagesOrganizationEntity.Name
                            : null
                    )
            )
            .ForMember(d => d.SubordinateId, o => o.MapFrom(s => s.SubordinateId))
            .ForMember(
                d => d.SubordinateName,
                o =>
                    o.MapFrom(s =>
                        s.Subordinate != null
                            ? s.Subordinate.FirstName + " " + s.Subordinate.LastName
                            : null
                    )
            )
            .ForMember(d => d.GroupId, o => o.MapFrom(s => s.GroupId));

        // CreateDTO → Model
        CreateMap<CreateEmployeeDTO, Employee>()
            .ForMember(dest => dest.PersonalInfo, opt => opt.MapFrom(src => src.PersonalInfo))
            .ForMember(dest => dest.CompanyInfo, opt => opt.MapFrom(src => src.CompanyInfo))
            .ForMember(dest => dest.CareerPathInfo, opt => opt.MapFrom(src => src.CareerPathInfo))
            .ForMember(dest => dest.TaxInfo, opt => opt.MapFrom(src => src.TaxInfo))
            .ForMember(dest => dest.InsuranceInfo, opt => opt.MapFrom(src => src.InsuranceInfo))
            .ForMember(
                dest => dest.EmergencyContactInfo,
                opt => opt.MapFrom(src => src.EmergencyContactInfo)
            )
            .ForMember(dest => dest.ScheduleInfo, opt => opt.MapFrom(src => src.ScheduleInfo))
            // RoleDetails cần xử lý riêng trong service
            .ForMember(dest => dest.RoleDetails, opt => opt.Ignore());

        // UpdateDTO → Model (chỉ map giá trị khác null)
        CreateMap<UpdateEmployeeDTO, Employee>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => src.MiddleName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.Avatar))
            .ForMember(dest => dest.PersonalInfo, opt => opt.MapFrom(src => src.PersonalInfo))
            .ForMember(dest => dest.CompanyInfo, opt => opt.MapFrom(src => src.CompanyInfo))
            .ForMember(dest => dest.CareerPathInfo, opt => opt.MapFrom(src => src.CareerPathInfo))
            .ForMember(dest => dest.TaxInfo, opt => opt.MapFrom(src => src.TaxInfo))
            .ForMember(dest => dest.InsuranceInfo, opt => opt.MapFrom(src => src.InsuranceInfo))
            .ForMember(
                dest => dest.EmergencyContactInfo,
                opt => opt.MapFrom(src => src.EmergencyContactInfo)
            )
            .ForMember(dest => dest.ScheduleInfo, opt => opt.MapFrom(src => src.ScheduleInfo))
            // Bỏ qua mapping tự động cho RoleDetails (xử lý trong service)
            .ForMember(dest => dest.RoleDetails, opt => opt.Ignore());

        // DTOs cho sub-fields
        CreateMap<PersonalInfoDTO, PersonalInfo>();
        CreateMap<CompanyInfoDTO, CompanyInfo>();
        CreateMap<CareerPathInfoDTO, CareerPathInfo>()
            .ForMember(d => d.Degrees, o => o.MapFrom(s => s.Degree))
            .ForMember(d => d.Certifications, o => o.MapFrom(s => s.Certification))
            .ForMember(d => d.Specializations, o => o.MapFrom(s => s.Specialization));
        CreateMap<TaxInfoDTO, TaxInfo>();
        CreateMap<InsuranceInfoDTO, InsuranceInfo>();
        CreateMap<EmergencyContactInfoDTO, EmergencyContactInfo>()
            .ForMember(d => d.Name, o => o.MapFrom(s => s.EmergencyContactName))
            .ForMember(d => d.Phone, o => o.MapFrom(s => s.EmergencyContactPhone))
            .ForMember(d => d.Relationship, o => o.MapFrom(s => s.Relationship))
            .ForMember(d => d.CurrentAddress, o => o.MapFrom(s => s.EmergencyContactCurrentAddress))
            .ForMember(
                d => d.PermanentAddress,
                o => o.MapFrom(s => s.EmergencyContactPermanentAddress)
            );
        CreateMap<ScheduleInfoDTO, ScheduleInfo>();
    }
}
