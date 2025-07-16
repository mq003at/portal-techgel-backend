namespace portal.Models;

public class WarehouseLocation : BaseModel
{
    public int WarehouseId { get; set; }
    public Warehouse Warehouse { get; set; } = null!;

    public string Zone { get; set; } = default!;
    public int Aisle { get; set; }
    public int Rack { get; set; }
    public string Shelf { get; set; } = default!;

    public string Code => $"{Zone}-{Aisle:D2}-{Rack:D2}-{Shelf}";

    public ICollection<StockLocation> StockLocations { get; set; } = new List<StockLocation>();
}
