namespace portal.Models;

public class Warehouse : BaseModel
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Location { get; set; } = null!;
    public int Capacity { get; set; }
    public int ManagerId { get; set; }
    public Employee Manager { get; set; } = null!;
    public bool IsProjectSite { get; set; } = false;
}
