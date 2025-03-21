namespace portal.DTOs;

using portal.Models;

public class DepartmentDTO : BaseDTO<Department>
{
    public string Name { get; set; } = null!;
    public int DivisionId { get; set; }
    public List<int> SectionIds { get; set; } = new();
    public List<SectionDTO> Sections { get; set; } = new();

    public override void UpdateModel(Department model)
    {
        model.Name = Name;
        model.DivisionId = DivisionId;
        model.MainID = MainID;
    }
}

public class UpdateDepartmentDTO : BaseDTO<Department>
{
    public string? Name { get; set; }
    public int? DivisionId { get; set; }

    public List<int> SectionIds { get; set; } = new();
    public List<SectionDTO> Sections { get; set; } = new();

    public override void UpdateModel(Department model)
    {
        if (Name != null)
            model.Name = Name;
        if (DivisionId.HasValue)
            model.DivisionId = DivisionId.Value;
        if (MainID != null)
            model.MainID = MainID;
    }
}
