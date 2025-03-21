using System.Text.Json.Serialization;

namespace portal.Models;

public class EmployeeSection
{
    public int EmployeeId { get; set; }
    [JsonIgnore]
    public Employee Employee { get; set; } = null!;

    public int SectionId { get; set; }
    public Section Section { get; set; } = null!;
}
