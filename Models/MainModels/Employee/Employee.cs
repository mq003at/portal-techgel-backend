namespace portal.Models;

public class Employee : BaseModel
{
    // Thông tin cơ bản
    public string FirstName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = null!;
    public string? Avatar { get; set; }

    // Owned types (sẽ map vào cùng bảng Employees)
    public PersonalInfo PersonalInfo { get; set; } = new();
    public CompanyInfo CompanyInfo { get; set; } = new();
    public CareerPathInfo CareerPathInfo { get; set; } = new();
    public TaxInfo TaxInfo { get; set; } = new();
    public InsuranceInfo InsuranceInfo { get; set; } = new();
    public EmergencyContactInfo EmergencyContactInfo { get; set; } = new();
    public ScheduleInfo ScheduleInfo { get; set; } = new();

    // Quan hệ many-to-many với OrganizationEntity
    public ICollection<OrganizationEntityEmployee> OrganizationEntityEmployees { get; set; } =
        new List<OrganizationEntityEmployee>();

    // Quan hệ 1-n với RoleDetail
    public ICollection<EmployeeRoleDetail> RoleDetails { get; set; } =
        new List<EmployeeRoleDetail>();
}
