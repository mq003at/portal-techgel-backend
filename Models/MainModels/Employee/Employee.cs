using System.ComponentModel.DataAnnotations.Schema;

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

    // Role Info
    public int? SupervisorId { get; set; }
    public Employee? Supervisor { get; set; }
    public int? DeputySupervisorId { get; set; }
    public Employee? DeputySupervisor { get; set; }
    public List<Employee> Subordinates { get; set; } = new List<Employee>();
    public List<Employee> DeputySubordinates { get; set; } = new List<Employee>();
    public List<OrganizationEntityEmployee> OrganizationEntityEmployees { get; set; } = new List<OrganizationEntityEmployee>();
    [NotMapped]
    public IEnumerable<Employee> AllSubordinates =>
        Subordinates.Concat(DeputySubordinates).Distinct();
}
