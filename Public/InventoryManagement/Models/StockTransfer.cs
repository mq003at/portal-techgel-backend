namespace portal.Models;

public class StockTransfer : BaseModel
{
    public string TransferNumber { get; set; } = default!;
    public int FromWarehouseId { get; set; }
    public int ToWarehouseId { get; set; }
    public DateTime TransferDate { get; set; }

    public List<StockTransferItem> Items { get; set; } = new();
}
