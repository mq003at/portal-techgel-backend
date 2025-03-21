namespace portal.Models;

public class Department : BaseModel
{
    public string Name { get; set; } = null!;

    public int DivisionId { get; set; }
    public Division Division { get; set; } = null!;

    public ICollection<Section> Sections { get; set; } = new List<Section>();
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
