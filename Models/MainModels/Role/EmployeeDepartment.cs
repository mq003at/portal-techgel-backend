using System.Text.Json.Serialization;

namespace portal.Models;

public class EmployeeDepartment
{
    public int EmployeeId { get; set; }
    [JsonIgnore]
    public Employee Employee { get; set; } = null!;

    public int DepartmentId { get; set; }
    public Department Department { get; set; } = null!;
}
