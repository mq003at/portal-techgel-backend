namespace portal.Models;

public class Team : BaseModel
{
    public string Name { get; set; } = null!;

    public int UnitId { get; set; }
    public Unit Unit { get; set; } = null!;
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
