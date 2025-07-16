namespace portal.Models;

public class InventoryTransactionLog : BaseModel
{
    public int MaterialId { get; set; }
    public Material Material { get; set; } = null!;

    public TransactionType TransactionType { get; set; } = default!; // Issue, Receive, Transfer, Adjust
    public decimal Quantity { get; set; }
    public int? RelatedEntityId { get; set; }
    public string? ReferenceNumber { get; set; }
    public DateTime TransactionDate { get; set; }
}

public enum TransactionType
{
    ISSUE,
    RECEIVE,
    TRANSFER,
    ADJUST
}
