using portal.Enums;
using portal.Models;

namespace portal.DTOs;

public class OrganizationEntityEmployeeDTO : BaseModelDTO
{
    public int OrganizationEntityId { get; set; }
    public string OrganizationEntityName { get; set; } = null!;
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = null!;

}

public class OrganizationEntityEmployeeCreateDTO : BaseModelCreateDTO
{
    public int OrganizationEntityId { get; set; }
    public int EmployeeId { get; set; }
}

public class OrganizationEntityEmployeeUpdateDTO : BaseModelUpdateDTO
{
    public int OrganizationEntityId { get; set; }
    public int EmployeeId { get; set; }
}