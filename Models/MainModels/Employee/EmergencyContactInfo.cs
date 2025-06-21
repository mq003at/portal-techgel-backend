namespace portal.Models;

public class EmergencyContactInfo : BaseModelWithOnlyId
{
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Relationship { get; set; }
    public string? CurrentAddress { get; set; }
}
