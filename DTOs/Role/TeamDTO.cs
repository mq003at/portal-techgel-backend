namespace portal.DTOs;

using portal.Models;

public class TeamDTO : BaseDTO<Team>
{
    public string Name { get; set; } = null!;
    public int UnitId { get; set; }

    public override void UpdateModel(Team model)
    {
        model.Name = Name;
        model.MainID = MainID;
        model.UnitId = UnitId;
    }
}

public class UpdateTeamDTO : BaseDTO<Team>
{
    public string? Name { get; set; }
    public int? UnitId { get; set; } // ✅ Nullable for partial updates

    public override void UpdateModel(Team model)
    {
        if (Name != null)
            model.Name = Name;
        if (UnitId.HasValue)
            model.UnitId = UnitId.Value;
        if (MainID != null)
            model.MainID = MainID;
    }
}
