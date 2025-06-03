using System.ComponentModel.DataAnnotations.Schema;
namespace portal.Models;

public class GatePass : BaseWorkflow
{
    public int EmployeeId { get; set; }
    [NotMapped]
    public string EmployeeName { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public ICollection<GatePassNodes> GatePassNodes { get; set; } = new List<GatePassNodes>();
}