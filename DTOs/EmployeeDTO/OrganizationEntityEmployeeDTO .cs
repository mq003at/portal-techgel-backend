using portal.Enums;
using portal.Models;

namespace portal.DTOs;

public class OrganizationEntityEmployeeDTO : BaseModelDTO
{
    public int OrganizationEntityId { get; set; }
    public int EmployeeId { get; set; }
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