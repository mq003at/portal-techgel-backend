using portal.Enums;
using portal.Models;

public class OrganizationEntityEmployee : BaseModel
{
    // Navigation to organization entity
    public int OrganizationEntityId { get; set; }
    public OrganizationEntity OrganizationEntity { get; set; } = null!;

    // Navigation to Employee
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    // Special role in that org
    public OrganizationRelationType OrganizationRelationType { get; set; }

    // In the future, this will replace Position field
    public bool IsPrimary { get; set; } = false;
}
