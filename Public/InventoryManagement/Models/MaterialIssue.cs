namespace portal.Models;

public class MaterialIssue : BaseModel
{
    public string IssueNumber { get; set; } = default!;
    public int WarehouseId { get; set; }
    public int ProjectId { get; set; }
    public DateTime IssueDate { get; set; }

    public List<MaterialIssueItem> Items { get; set; } = new();
}
