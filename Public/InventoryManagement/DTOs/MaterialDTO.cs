using System.ComponentModel.DataAnnotations;
using portal.Enums;
using portal.Models;

namespace portal.DTOs;

public class MaterialDTO : BaseModelDTO
{
    public string Code { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Unit { get; set; } = default!;
    public MaterialType Type { get; set; }
    public string? Brand { get; set; }
    public string? Specification { get; set; }
}

public class CreateMaterialDTO : BaseModelCreateDTO
{
    [Required]
    public string Code { get; set; } = default!;

    [Required]
    public string Name { get; set; } = default!;

    [Required]
    public string Unit { get; set; } = default!;

    public MaterialType Type { get; set; }
    public string? Brand { get; set; }
    public string? Specification { get; set; }
}

public class UpdateMaterialDTO : BaseModelUpdateDTO
{
    public string? Name { get; set; }
    public MaterialType? Type { get; set; }
    public string? Brand { get; set; }
    public string? Specification { get; set; }
}
