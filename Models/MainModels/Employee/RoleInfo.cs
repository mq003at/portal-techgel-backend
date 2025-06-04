using System.ComponentModel.DataAnnotations.Schema;
using portal.Models;

public class RoleInfo
{
    // Supervisor relationship (one-to-many)
    public int? SupervisorId { get; set; }
    [NotMapped]
    public Employee? Supervisor { get; set; }

    public List<int> SubordinateIds { get; set; } = new List<int>();
    // Subordinates relationship (one-to-many)    
    [NotMapped]
    public List<Employee> Subordinates { get; set; } = new List<Employee>();

    // Entities this role manages (many-to-many)
    public List<int> ManagedOrganizationEntityIds { get; set; } = new List<int>();
    [NotMapped]
    public List<OrganizationEntity> ManagedOrganizationEntities { get; set; } =
        new List<OrganizationEntity>();


    [NotMapped]
    public List<OrganizationEntityEmployee> OrganizationEntityEmployees { get; set; } =
        new List<OrganizationEntityEmployee>();

    // Optional grouping of roles into a higher-level Group
    public int? GroupId { get; set; }
}
