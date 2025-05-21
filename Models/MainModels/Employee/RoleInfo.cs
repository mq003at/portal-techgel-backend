using portal.Models;

public class RoleInfo
{
    public int? SupervisorId { get; set; }
    public Employee? Supervisor { get; set; }
    public ICollection<EmployeeRoleDetail> RoleDetailsInfo { get; set; } =
        new List<EmployeeRoleDetail>();
}

public class EmployeeRoleDetail
{
    public int OrganizationEntityId { get; set; }
    public OrganizationEntity OrganizationEntity { get; set; } = null!;

    public int? ManagesOrganizationEntityId { get; set; }
    public OrganizationEntity? ManagesOrganizationEntity { get; set; }

    public int? SubordinateId { get; set; }
    public Employee? Subordinate { get; set; }

    public int? GroupId { get; set; }

    // public Group? Group { get; set; }

    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;
}
