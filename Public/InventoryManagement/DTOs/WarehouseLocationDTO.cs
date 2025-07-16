using System.ComponentModel.DataAnnotations;
using portal.DTOs;
using portal.Models;

namespace portal.Services;

public class WarehouseLocationDTO : BaseModelDTO
{
    public int WarehouseId { get; set; }
    public string WarehouseName { get; set; } = default!;

    public string Zone { get; set; } = default!;
    public int Aisle { get; set; }
    public int Rack { get; set; }
    public string Shelf { get; set; } = default!;

    public string Code => $"{Zone}-{Aisle:D2}-{Rack:D2}-{Shelf}";
}

public class WarehouseLocationCreateDTO : BaseModelCreateDTO
{
    [Required]
    public int WarehouseId { get; set; }

    [Required]
    public string Zone { get; set; } = default!;

    [Range(1, int.MaxValue)]
    public int Aisle { get; set; }

    [Range(1, int.MaxValue)]
    public int Rack { get; set; }

    [Required]
    public string Shelf { get; set; } = default!;
}

public class WarehouseLocationUpdateDTO : BaseModelUpdateDTO
{
    public string? Zone { get; set; }
    public int? Aisle { get; set; }
    public int? Rack { get; set; }
    public string? Shelf { get; set; }
}
