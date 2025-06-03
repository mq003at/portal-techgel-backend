using System.ComponentModel.DataAnnotations.Schema;

namespace portal.Models;

public class LeaveRequestWorkflow : BaseWorkflow
{
    public int EmployeeId { get; set; }
    [NotMapped]
    public Employee Employee { get; set; } = new();
    [NotMapped]
    public string EmployeeName { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public int TotalDays => (EndDate - StartDate).Days + 1;
    public float EmployeeAnnualLeaveTotalDays { get; set; } = 0f;

    public ICollection<LeaveRequestNode> LeaveRequestNodes { get; set; } = new List<LeaveRequestNode>();
}
