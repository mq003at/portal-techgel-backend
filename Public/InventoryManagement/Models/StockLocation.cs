namespace portal.Models;

public class StockLocation : BaseModel
{
    public int StockId { get; set; }
    public Stock Stock { get; set; } = null!;

    public int WarehouseLocationId { get; set; }
    public WarehouseLocation WarehouseLocation { get; set; } = null!;

    public decimal Quantity { get; set; } // Qty at that specific shelf
}
