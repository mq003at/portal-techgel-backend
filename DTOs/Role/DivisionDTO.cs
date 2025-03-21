namespace portal.DTOs;

using System.Text.Json.Serialization;
using portal.Models;

public class DivisionDTO : BaseDTO<Division>
{
    public string Name { get; set; } = null!;

    public List<int> DepartmentIds { get; set; } = new();

    public List<DepartmentDTO> Departments { get; set; } = new();

    public override void UpdateModel(Division model)
    {
        model.Name = Name;
        model.MainID = MainID;
    }
}

public class UpdateDivisionDTO : BaseDTO<Division>
{
    public string? Name { get; set; }
    public List<int> DepartmentIds { get; set; } = new();

    public List<DepartmentDTO> Departments { get; set; } = new();

    public override void UpdateModel(Division model)
    {
        if (Name != null)
            model.Name = Name;
        if (MainID != null)
            model.MainID = MainID;
    }
}
