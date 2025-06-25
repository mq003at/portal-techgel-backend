using System.ComponentModel.DataAnnotations.Schema;
using portal.Enums;

namespace portal.Models;

public class OrganizationEntity : BaseModel
{
    public int Level { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public OrganizationStatus Status { get; set; }
    public int? SortOrder { get; set; }
    [ForeignKey("Parent")]
    public int? ParentId { get; set; }
    public OrganizationEntity? Parent { get; set; }
    public List<OrganizationEntity>? Children { get; set; } = new List<OrganizationEntity>();
    public List<Employee> Employees { get; set; } = new List<Employee>();
    public List<OrganizationEntityEmployee> OrganizationEntityEmployees { get; set; } =
        new List<OrganizationEntityEmployee>();
}
