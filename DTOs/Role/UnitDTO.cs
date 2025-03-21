namespace portal.DTOs;

using System.Text.Json.Serialization;
using portal.Models;

public class UnitDTO : BaseDTO<Unit>
{
    public string Name { get; set; } = null!;
    public int SectionId { get; set; }
    public List<int> TeamIds { get; set; } = new();
    public List<TeamDTO> Teams { get; set; } = new();

    public override void UpdateModel(Unit model)
    {
        model.Name = Name;
        model.MainID = MainID;
        model.SectionId = SectionId;
    }
}

public class UpdateUnitDTO : BaseDTO<Unit>
{
    public string? Name { get; set; }
    public int? SectionId { get; set; }
    public List<int> TeamIds { get; set; } = new();

    [JsonIgnore]
    public List<TeamDTO> Teams { get; set; } = new();

    public override void UpdateModel(Unit model)
    {
        if (Name != null)
            model.Name = Name;
        if (MainID != null)
            model.MainID = MainID;
        if (SectionId.HasValue)
            model.SectionId = SectionId.Value;
    }
}
