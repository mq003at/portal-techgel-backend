using Microsoft.EntityFrameworkCore;
using portal.Enums;

namespace portal.Models;

public class GeneralDocumentInfo
{
    public string Name { get; set; } = null!;
    public DocumentTypeEnum Type { get; set; }
    public DocumentStatusEnum Status { get; set; }
    public DocumentSubTypeEnum SubType { get; set; }
    public DocumentCategoryEnum Category { get; set; }

    public int OwnerId { get; set; }
    public string OwnerName { get; set; } = string.Empty;

    public int OrganizationEntityResponsibleId { get; set; }
    public string OrganizationEntityResponsibleName { get; set; } = string.Empty;

    public ICollection<string> Tag { get; set; } = new List<string>();
    public string Description { get; set; } = null!;
    public string Url { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;

    public ICollection<int> GeneralWorkflowIds { get; set; } = new List<int>();
}
