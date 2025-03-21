namespace portal.Models;

public class Unit : BaseModel
{
    public string Name { get; set; } = null!;

    public int SectionId { get; set; }
    public Section Section { get; set; } = null!;

    public ICollection<Team> Teams { get; set; } = new List<Team>();
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
