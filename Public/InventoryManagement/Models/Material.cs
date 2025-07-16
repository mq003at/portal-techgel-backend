using portal.Enums;

namespace portal.Models;

public class Material : BaseModel
{
    public string Code { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Unit { get; set; } = default!;
    public string? Specification { get; set; }
    public string? Brand { get; set; }
    public MaterialType Type { get; set; } // Enum: Consumable, Reusable, etc.
    public bool IsHazardous { get; set; } = false;
}
