using System.Text.Json.Serialization;

namespace portal.Models;

public class EmployeeTeam
{
    public int EmployeeId { get; set; }
    [JsonIgnore]
    public Employee Employee { get; set; } = null!;

    public int TeamId { get; set; }
    public Team Team { get; set; } = null!;
}
