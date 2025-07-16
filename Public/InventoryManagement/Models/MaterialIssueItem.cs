namespace portal.Models;

public class MaterialIssueItem : BaseModel
{
    public int MaterialIssueId { get; set; }
    public MaterialIssue MaterialIssue { get; set; } = null!;

    public int MaterialId { get; set; }
    public Material Material { get; set; } = null!;

    public decimal QuantityIssued { get; set; }
}
