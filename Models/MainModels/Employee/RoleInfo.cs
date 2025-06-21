using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using portal.Models;

public class RoleInfo : BaseModelWithOnlyId
{
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    // Supervisor (can be null if top-level)
    public int? SupervisorId { get; set; }
    public Employee? Supervisor { get; set; }

    // Deputy Supervisor (optional)
    public int? DeputySupervisorId { get; set; }
    public Employee? DeputySupervisor { get; set; }

    // Subordinates (many-to-many logic may live elsewhere if needed)
    public List<int> SubordinateIds { get; set; } = new();
    public List<Employee> Subordinates { get; set; } = new();

    // Managed organization units
    public List<int> ManagedOrganizationEntityIds { get; set; } = new();
    public List<OrganizationEntity> ManagedOrganizationEntities { get; set; } = new();

    // Direct links (e.g. for assignments)
    public List<OrganizationEntityEmployee> OrganizationEntityEmployees { get; set; } = new();

    // Grouping (optional)
    public int? GroupId { get; set; }
}
