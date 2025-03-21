namespace portal.Models;

public class EmployeeUnit
{
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    public int UnitId { get; set; }
    public Unit Unit { get; set; } = null!;
}
