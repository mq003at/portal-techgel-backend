using portal.Enums;
using portal.Models;

public class OrganizationEntityEmployee : BaseModel
{
    public int OrganizationEntityId { get; set; }
    public OrganizationEntity OrganizationEntity { get; set; } = null!;

    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;
    public OrganizationRelationType OrganizationRelationType { get; set; }
    public bool IsPrimary { get; set; } = false;
}
