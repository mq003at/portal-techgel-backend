using portal.Models;

namespace portal.DTOs;

public class RoleInfoDTO
{
    public int? SupervisorId { get; set; }

    public string? SupervisorName { get; set; }

    public List<int>? SubordinateIds { get; set; } = new List<int>();

    public List<string>? SubordinateNames { get; set; } = new List<string>();

    public List<int>? ManagedOrganizationEntityIds { get; set; } = new List<int>();

    public List<string>? ManagedOrganizationEntityNames { get; set; } = new List<string>();
    public List<int>? OrganizationEntityIds { get; set; } = new List<int>();
    public List<string>? OrganizationEntityNames { get; set; } = new List<string>();

    public int? GroupId { get; set; }
}
