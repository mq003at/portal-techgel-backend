using System.ComponentModel.DataAnnotations.Schema;
using portal.Enums;
using portal.Models;



[Table("OrganizationEntityEmployees")]
public class OrganizationEntityEmployee : BaseModel
{

    public int OrganizationEntityId { get; set; }
    public OrganizationEntity OrganizationEntity { get; set; } = null!;
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

}
