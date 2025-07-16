using System.ComponentModel.DataAnnotations;

public class StockLocationDTO
{
    public int WarehouseLocationId { get; set; }
    public string LocationCode { get; set; } = default!;
    public decimal Quantity { get; set; }
}

public class StockLocationCreateDTO
{
    [Required]
    public int WarehouseLocationId { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Quantity { get; set; }
}
