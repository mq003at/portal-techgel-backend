namespace portal.DTOs;

using System.ComponentModel.DataAnnotations;

public class StockDTO : BaseModelDTO
{
    public int MaterialId { get; set; }
    public string MaterialName { get; set; } = default!;

    public int WarehouseId { get; set; }
    public string WarehouseName { get; set; } = default!;

    public decimal TotalQuantity => Locations?.Sum(l => l.Quantity) ?? 0;

    public List<StockLocationDTO> Locations { get; set; } = new();
}

public class StockCreateDTO : BaseModelCreateDTO
{
    [Required]
    public int MaterialId { get; set; }

    [Required]
    public int WarehouseId { get; set; }

    [Required]
    public List<StockLocationCreateDTO> Locations { get; set; } = new();
}

public class StockUpdateDTO : BaseModelUpdateDTO
{
    public int? MaterialId { get; set; }
    public int? WarehouseId { get; set; }

    public List<StockLocationCreateDTO>? Locations { get; set; }
}
