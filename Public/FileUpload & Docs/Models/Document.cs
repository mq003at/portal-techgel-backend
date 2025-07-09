using portal.Enums;

namespace portal.Models;

public class Document : BaseModel
{
    public string Name { get; set; } = null!;
    public DocumentStatusEnum Status { get; set; } = DocumentStatusEnum.UNKNOWN;
    public DocumentCategoryEnum Category { get; set; }
    public string Location { get; set; } = null!;
    public string FileExtension { get; set; } = null!;
    public long SizeInBytes { get; set; }
    public string? TemplateKey { get; set; }
    public List<string> Tag { get; set; } = new List<string>();
    public string Description { get; set; } = null!;
    public string Url { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public long DownloadCount { get; set; } = 0;
    public List<int> RestrictedEmployeeIds { get; set; } = new List<int>();
    public List<DocumentAssociation> DocumentAssociations { get; set; } =
        new List<DocumentAssociation>();
}
