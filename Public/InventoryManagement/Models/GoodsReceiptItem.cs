namespace portal.Models;

public class GoodsReceiptItem : BaseModel
{
    public int GoodsReceiptNoteId { get; set; }
    public GoodsReceiptNote GoodsReceiptNote { get; set; } = null!;

    public int MaterialId { get; set; }
    public Material Material { get; set; } = null!;

    public decimal QuantityReceived { get; set; }
}
