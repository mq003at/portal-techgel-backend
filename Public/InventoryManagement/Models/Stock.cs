namespace portal.Models;

public class Stock : BaseModel
{
    public int MaterialId { get; set; }
    public Material Material { get; set; } = null!;

    public int WarehouseId { get; set; }
    public Warehouse Warehouse { get; set; } = null!;

    public decimal TotalQuantity { get; set; }
    public decimal? ReservedQuantity { get; set; }
    public ICollection<StockLocation> StockLocations { get; set; } = new List<StockLocation>();
}
