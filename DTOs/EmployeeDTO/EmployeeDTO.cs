using System.ComponentModel.DataAnnotations;
using portal.DTOs;
using portal.Models;

namespace portal.DTOs;

using System.ComponentModel.DataAnnotations;
using portal.Enums;
using portal.Models;

public class EmployeeDTO : BaseModelDTO
{
    // Thông tin cơ bản
    public string FirstName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = null!;
    public string? Avatar { get; set; }

    public string? Password { get; set; }

    // Các sub-DTO
    public PersonalInfoDTO PersonalInfo { get; set; } = null!;
    public CompanyInfoDTO CompanyInfo { get; set; } = null!;
    public CareerPathInfoDTO CareerPathInfo { get; set; } = null!;
    public TaxInfoDTO TaxInfo { get; set; } = null!;
    public InsuranceInfoDTO InsuranceInfo { get; set; } = null!;
    public EmergencyContactInfoDTO EmergencyContactInfo { get; set; } = null!;
    public ScheduleInfoDTO ScheduleInfo { get; set; } = null!;
    public Signature? Signature { get; set; }

    // Role Info
    public RoleInfoDTO RoleInfo { get; set; } = null!;
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

    [Required]
    public CompanyInfoDTO CompanyInfo { get; set; } = null!;

    [Required]
    public CareerPathInfoDTO CareerPathInfo { get; set; } = null!;

    [Required]
    public TaxInfoDTO TaxInfo { get; set; } = null!;

    [Required]
    public InsuranceInfoDTO InsuranceInfo { get; set; } = null!;

    [Required]
    public EmergencyContactInfoDTO EmergencyContactInfo { get; set; } = null!;
    public ScheduleInfoDTO ScheduleInfo { get; set; } = null!;
    public Signature? Signature { get; set; }

    [Required]
    public RoleInfoDTO RoleInfo { get; set; } = null!;
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
    public EmergencyContactInfoDTO? EmergencyContactInfo { get; set; }
    public ScheduleInfoDTO? ScheduleInfo { get; set; }

    public RoleInfoDTO? RoleInfo { get; set; }
    public Signature? Signature { get; set; }
}

public class LoginRequestDTO
{
    public string MainId { get; set; } = null!;
    public string Password { get; set; } = null!;
}

// public class EmployeeShortDTO
// {
//     public int Id { get; set; }
//     public string MainId { get; set; } = null!;
//     public string FirstName { get; set; } = null!;
//     public string MiddleName { get; set; } = null!;
//     public string LastName { get; set; } = null!;
//     public string FullName => $"{FirstName} {MiddleName} {LastName}";
//     public string? Avatar { get; set; }
//     public string Department { get; set; } = null!;
//     public string Position { get; set; } = null!;
// }



