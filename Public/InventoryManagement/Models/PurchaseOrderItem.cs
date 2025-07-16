namespace portal.Models;

public class PurchaseOrderItem : BaseModel
{
    public int PurchaseOrderId { get; set; }
    public PurchaseOrder PurchaseOrder { get; set; } = null!;

    public int MaterialId { get; set; }
    public Material Material { get; set; } = null!;

    public decimal QuantityOrdered { get; set; }
    public decimal QuantityReceived { get; set; }
}
