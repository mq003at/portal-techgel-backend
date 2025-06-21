using System.ComponentModel.DataAnnotations;
using portal.Models;

namespace portal.DTOs;

public class RoleInfoDTO : BaseModelWithOnlyIdDTO
{
    public int? SupervisorId { get; set; }

    public string? SupervisorName { get; set; }

    public int? DeputySupervisorId { get; set; }
    public string? DeputySupervisorName { get; set; }

    public List<int>? SubordinateIds { get; set; } = new List<int>();

    public List<string>? SubordinateNames { get; set; } = new List<string>();

    public List<int>? ManagedOrganizationEntityIds { get; set; } = new List<int>();

    public List<string>? ManagedOrganizationEntityNames { get; set; } = new List<string>();
    public List<int>? OrganizationEntityIds { get; set; } = new List<int>();
    public List<string>? OrganizationEntityNames { get; set; } = new List<string>();

    public int? GroupId { get; set; }
}

public class RoleInfoCreateDTO : BaseModelWithOnlyIdCreateDTO
{
    [Required]
    public int SupervisorId { get; set; }
    [Required]
    public int DeputySupervisorId { get; set; }
    public List<int> OrganizationEntityIds { get; set; } = new List<int>();
    public List<int> ManagedOrganizationEntityIds { get; set; } = new List<int>();
    public int? GroupId { get; set; }
}

public class RoleInfoUpdateDTO : BaseModelWithOnlyIdUpdateDTO
{
    public int? SupervisorId { get; set; }

    public int? DeputySupervisorId { get; set; }

    public List<int>? OrganizationEntityIds { get; set; } = new List<int>();
    public List<int>? ManagedOrganizationEntityIds { get; set; } = new List<int>();
    public int? GroupId { get; set; }
}
