using System.Text.Json.Serialization;

namespace portal.Models;

public class EmployeeDivision
{
    public int EmployeeId { get; set; }
    [JsonIgnore]
    public Employee Employee { get; set; } = null!;

    public int DivisionId { get; set; }
    public Division Division { get; set; } = null!;
}
