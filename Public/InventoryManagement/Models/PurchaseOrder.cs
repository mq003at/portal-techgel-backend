namespace portal.Models;

public class PurchaseOrder : BaseModel
{
    public string OrderNumber { get; set; } = default!;
    public int SupplierId { get; set; } // Assume there's a Supplier table
    public DateTime OrderDate { get; set; }
    public POStatus Status { get; set; }

    public List<PurchaseOrderItem> Items { get; set; } = new();
}

public enum POStatus
{
    PENDING,
    APPROVED,
    RECEIVED,
    CANCELLED
}
