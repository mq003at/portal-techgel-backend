namespace portal.Models;

public class MaterialRequestItem : BaseModel
{
    public int MaterialRequestId { get; set; }
    public MaterialRequest MaterialRequest { get; set; } = null!;

    public int MaterialId { get; set; }
    public Material Material { get; set; } = null!;

    public decimal QuantityRequested { get; set; }
    public decimal QuantityIssued { get; set; } = 0;
}
