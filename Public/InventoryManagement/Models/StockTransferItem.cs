namespace portal.Models;

public class StockTransferItem : BaseModel
{
    public int StockTransferId { get; set; }
    public StockTransfer StockTransfer { get; set; } = null!;

    public int MaterialId { get; set; }
    public Material Material { get; set; } = null!;

    public decimal QuantityTransferred { get; set; }
}
