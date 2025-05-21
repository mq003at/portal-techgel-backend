namespace portal.Models;

public class EmployeeRoleDetail
{
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    public int OrganizationEntityId { get; set; }
    public OrganizationEntity OrganizationEntity { get; set; } = null!;

    public int? ManagesOrganizationEntityId { get; set; }
    public OrganizationEntity? ManagesOrganizationEntity { get; set; }

    public int? SubordinateId { get; set; }
    public Employee? Subordinate { get; set; }

    public int? GroupId { get; set; }
    // nếu có entity Group, bạn có thể nối thêm navigation ở đây
}
