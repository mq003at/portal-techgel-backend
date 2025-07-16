using System.ComponentModel.DataAnnotations;

namespace portal.DTOs;

public class WarehouseDTO : BaseModelDTO
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? Location { get; set; }
    public int ManagerId { get; set; }
    public string ManagerName { get; set; } = default!;
    public int Capacity { get; set; }
    public bool IsProjectSite { get; set; }
}

public class CreateWarehouseDTO : BaseModelCreateDTO
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Description { get; set; } = null!;

    public string? Location { get; set; }

    [Required]
    public int ManagerId { get; set; }

    [Range(1, int.MaxValue)]
    public int Capacity { get; set; }
    public bool IsProjectSite { get; set; } = false;
}

public class UpdateWarehouseDTO : BaseModelUpdateDTO
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Location { get; set; }
    public int? ManagerId { get; set; }

    [Range(1, int.MaxValue)]
    public int? Capacity { get; set; }
    public bool? IsProjectSite { get; set; } = false;
}
