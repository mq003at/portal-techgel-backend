namespace portal.Models;

public class GoodsReceiptNote : BaseModel
{
    public string GRNNumber { get; set; } = default!;
    public int PurchaseOrderId { get; set; }
    public PurchaseOrder PurchaseOrder { get; set; } = null!;

    public int WarehouseId { get; set; }
    public DateTime ReceivedDate { get; set; }

    public List<GoodsReceiptItem> Items { get; set; } = new();
}
