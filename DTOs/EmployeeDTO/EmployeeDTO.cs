using System.ComponentModel.DataAnnotations;
using portal.DTOs;
using portal.Models;

namespace portal.DTOs;

using System.ComponentModel.DataAnnotations;
using portal.Enums;
using portal.Models;

public class EmployeeDTO : BaseModelDTO
{
    // BasicInfo
    public string FirstName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = null!;
    public string? Avatar { get; set; }

    public string? Password { get; set; }

    // sub-DTO
    public PersonalInfoDTO PersonalInfo { get; set; } = null!;
    public CompanyInfoDTO? CompanyInfo { get; set; }
    public CareerPathInfoDTO? CareerPathInfo { get; set; }
    public TaxInfoDTO? TaxInfo { get; set; }
    public InsuranceInfoDTO? InsuranceInfo { get; set; }
    public List<EmergencyContactInfoDTO>? EmergencyContactInfos { get; set; }
    public List<EmployeeQualificationInfoDTO>? EmployeeQualificationInfos { get; set; }
    public ScheduleInfoDTO? ScheduleInfo { get; set; }
    public SignatureDTO? Signature { get; set; }
    public List<OrganizationEntityEmployeeDTO>? OrganizationEntitiesEmployees { get; set; }

    // Role Info
    public int? SupervisorId { get; set; }
    public string? SupervisorName { get; set; }
    public int? DeputySupervisorId { get; set; }
    public string? DeputySupervisorName { get; set; }
    public List<int> SubordinateIds { get; set; } = new();
    public List<string> SubordinateNames { get; set; } = new();
    public List<int> DeputySubordinateIds { get; set; } = new();
    public List<string> DeputySubordinateNames { get; set; } = new();
    public List<OrganizationEntityDTO> OrganizationEntities { get; set; } = new();
}

public class CreateEmployeeDTO : BaseModelCreateDTO
{
    [Required]
    public string FirstName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public string? Password { get; set; }

    [Required]
    public string LastName { get; set; } = null!;
    public string? Avatar { get; set; }

    [Required]
    public PersonalInfoDTO PersonalInfo { get; set; } = null!;

    public int? SupervisorId { get; set; }
    public int? DeputySupervisorId { get; set; }
}

public class UpdateEmployeeDTO : BaseModelUpdateDTO
{
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? Avatar { get; set; }
    public string? Password { get; set; }
    public PersonalInfoDTO? PersonalInfo { get; set; }
    public CompanyInfoDTO? CompanyInfo { get; set; }
    public CareerPathInfoDTO? CareerPathInfo { get; set; }
    public TaxInfoDTO? TaxInfo { get; set; }
    public InsuranceInfoDTO? InsuranceInfo { get; set; }
    public List<EmergencyContactInfoDTO>? EmergencyContactInfo { get; set; }
    public ScheduleInfoDTO? ScheduleInfo { get; set; }

    public Signature? Signature { get; set; }
}

public class UpdateEmployeeDetailsDTO
{
    public UpdatePersonalInfoDTO? PersonalInfo { get; set; }
    public CompanyInfoUpdateDTO? CompanyInfo { get; set; }
    public UpdateScheduleInfoDTO? ScheduleInfo { get; set; }
    public UpdateCareerPathInfoDTO? CareerPathInfo { get; set; }
    public UpdateTaxInfoDTO? TaxInfo { get; set; }
    public InsuranceInfoUpdateDTO? InsuranceInfo { get; set; }

    public List<CreateEmergencyContactInfoDTO>? EmergencyContactInfos { get; set; }
    public List<CreateEmployeeQualificationInfoDTO>? EmployeeQualificationInfos { get; set; }

    // RoleInfo Update
    public int? SupervisorId { get; set; }
    public int? DeputySupervisorId { get; set; }
    public List<int>? SubordinateIds { get; set; }
    public List<int>? DeputySubordinateIds { get; set; }
}

public class LoginRequestDTO
{
    public string MainId { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class LoginResponseDTO
{
    public string Token { get; set; } = null!;
    public EmployeeDTO Employee { get; set; } = null!;
}

public class ChangePasswordDTO
{
    [Required]
    public string OldPassword { get; set; } = null!;

    [Required]
    public string NewPassword { get; set; } = null!;
}
