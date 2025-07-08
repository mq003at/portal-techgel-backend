namespace portal.Models;

public class Signature : BaseModel
{
    // FK back to Employee
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    // File metadata
    public string FileName { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public long FileSize { get; set; }
    public string StoragePath { get; set; } = null!;
    public DateTime UploadedAt { get; set; }
}
