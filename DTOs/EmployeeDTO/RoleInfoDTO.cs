using portal.Models;

namespace portal.DTOs;

public class RoleInfoDTO
{
    public int? SupervisorId { get; set; }

    public string? SupervisorName { get; set; }

    public ICollection<int>? SubordinateIds { get; set; } = new List<int>();

    public ICollection<string>? SubordinateNames { get; set; } = new List<string>();

    public ICollection<int>? ManagedOrganizationEntityIds { get; set; } = new List<int>();

    public ICollection<string>? ManagedOrganizationEntityNames { get; set; } = new List<string>();
    public ICollection<int>? OrganizationEntityIds { get; set; } = new List<int>();
    public ICollection<string>? OrganizationEntityNames { get; set; } = new List<string>();

    public int? GroupId { get; set; }
}
