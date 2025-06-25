using portal.Enums;
using portal.Models;

namespace portal.DTOs;

public class OrganizationEntityEmployeeDTO : BaseModelDTO
{
    public int OrganizationEntityId { get; set; }
    public string OrganizationEntityName { get; set; } = null!;
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = null!;
    public OrganizationRelationType OrganizationRelationType { get; set; }
    public bool IsPrimary { get; set; }
}

public class CreateOrganizationEntityEmployeeDTO : BaseModelCreateDTO
{
    public int OrganizationEntityId { get; set; }
    public int EmployeeId { get; set; }
    public OrganizationRelationType OrganizationRelationType { get; set; }
    public bool IsPrimary { get; set; }
}

public class UpdateOrganizationEntityEmployeeDTO : BaseModelUpdateDTO
{
    public int OrganizationEntityId { get; set; }
    public int EmployeeId { get; set; }
    public OrganizationRelationType OrganizationRelationType { get; set; }
    public bool IsPrimary { get; set; }
}