namespace portal.DTOs;

public class RoleDetailsInfoDTO
{
    public int OrganizationEntityId { get; set; }
    public string OrganizationEntityName { get; set; } = null!;

    public int? ManagesOrganizationEntityId { get; set; }
    public string? ManagesOrganizationEntityName { get; set; }

    public int? SubordinateId { get; set; }
    public string? SubordinateName { get; set; }

    public int? GroupId { get; set; }
}

public class RoleInfoDTO
{
    public int? SupervisorId { get; set; }
    public string? SupervisorName { get; set; }

    public List<RoleDetailsInfoDTO>? RoleDetailsInfo { get; set; }
}
