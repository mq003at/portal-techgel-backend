namespace portal.Models;

public class MaterialRequest : BaseModel
{
    public string RequestNumber { get; set; } = default!;
    public int ProjectId { get; set; }
    public DateTime RequestedDate { get; set; }
    public RequestStatus Status { get; set; } // Pending, Approved, Issued, Rejected

    public List<MaterialRequestItem> Items { get; set; } = new();
}

public enum RequestStatus
{
    PENDING,
    APPROVED,
    REJECTED,
    ISSUED
}
