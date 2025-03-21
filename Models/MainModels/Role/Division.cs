namespace portal.Models;

public class Division : BaseModel
{
    public string Name { get; set; } = null!;

    public ICollection<Department> Departments { get; set; } = new List<Department>();
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
