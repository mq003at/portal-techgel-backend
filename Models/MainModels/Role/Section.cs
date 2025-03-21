namespace portal.Models;

public class Section : BaseModel
{
    public string Name { get; set; } = null!;

    public int DepartmentId { get; set; }
    public Department Department { get; set; } = null!;

    public ICollection<Unit> Units { get; set; } = new List<Unit>();
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
