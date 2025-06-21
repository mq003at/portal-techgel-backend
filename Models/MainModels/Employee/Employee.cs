namespace portal.Models;

public class Employee : BaseModel
{
    // Thông tin cơ bản
    public string FirstName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = null!;
    public string? Avatar { get; set; }
    public string? Password { get; set; }

    // Owned types (sẽ map vào cùng bảng Employees)
    public PersonalInfo PersonalInfo { get; set; } = null!;
    public CompanyInfo? CompanyInfo { get; set; }
    public CareerPathInfo? CareerPathInfo { get; set; }
    public List<EmergencyContactInfo>? EmergencyContactInfos { get; set; }

    public List<EmployeeQualificationInfo>? EmployeeQualificationInfos { get; set; } 
    public TaxInfo? TaxInfo { get; set; }
    public InsuranceInfo? InsuranceInfo { get; set; }
    public ScheduleInfo? ScheduleInfo { get; set; }
    public Signature? Signature { get; set; }
    public RoleInfo? RoleInfo { get; set; } 
}
