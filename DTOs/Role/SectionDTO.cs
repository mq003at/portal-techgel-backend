namespace portal.DTOs;

using System.Text.Json.Serialization;
using portal.Models;

public class SectionDTO : BaseDTO<Section>
{
    public string Name { get; set; } = null!;
    public int DepartmentId { get; set; }
    public List<int> UnitIds { get; set; } = new();
    public List<UnitDTO> Units { get; set; } = new();

    public override void UpdateModel(Section model)
    {
        model.Name = Name;
        model.MainID = MainID;
        model.DepartmentId = DepartmentId;
    }
}

public class UpdateSectionDTO : BaseDTO<Section>
{
    public string? Name { get; set; } = null!;
    public int? DepartmentId { get; set; }
    public List<int>? UnitIds { get; set; } = new();
    public List<UnitDTO>? Units { get; set; } = new();

    public override void UpdateModel(Section model)
    {
        if (Name != null)
            model.Name = Name;
        if (MainID != null)
            model.MainID = MainID;
        if (DepartmentId.HasValue)
            model.DepartmentId = DepartmentId.Value;
    }
}
