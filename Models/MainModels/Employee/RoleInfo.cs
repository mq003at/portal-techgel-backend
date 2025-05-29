using portal.Models;

public class RoleInfo
{
    // Supervisor relationship (one-to-many)
    public int? SupervisorId { get; set; }
    public Employee? Supervisor { get; set; }

    // All direct reports
    public ICollection<Employee> Subordinates { get; set; } = new HashSet<Employee>();

    // Entities this role manages (many-to-many)
    public ICollection<OrganizationEntity> ManagedOrganizationEntities { get; set; } =
        new HashSet<OrganizationEntity>();

    public ICollection<OrganizationEntityEmployee> OrganizationEntityEmployees { get; set; } =
        new HashSet<OrganizationEntityEmployee>();

    // Optional grouping of roles into a higher-level Group
    public int? GroupId { get; set; }
}
