using System.ComponentModel.DataAnnotations.Schema;
using portal.Models;

public class RoleInfo
{
    // Supervisor relationship (one-to-many)
    public int? SupervisorId { get; set; }
    [NotMapped]
    public Employee? Supervisor { get; set; }

    public ICollection<int> SubordinateIds { get; set; } = new List<int>();
    // Subordinates relationship (one-to-many)    
    [NotMapped]
    public ICollection<Employee> Subordinates { get; set; } = new HashSet<Employee>();

    // Entities this role manages (many-to-many)
    public ICollection<int> ManagedOrganizationEntityIds { get; set; } = new List<int>();
    [NotMapped]
    public ICollection<OrganizationEntity> ManagedOrganizationEntities { get; set; } =
        new HashSet<OrganizationEntity>();


    [NotMapped]
    public ICollection<OrganizationEntityEmployee> OrganizationEntityEmployees { get; set; } =
        new HashSet<OrganizationEntityEmployee>();

    // Optional grouping of roles into a higher-level Group
    public int? GroupId { get; set; }
}
