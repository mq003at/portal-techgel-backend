using Microsoft.EntityFrameworkCore;
using portal.Enums;

namespace portal.Models;

public class GeneralDocumentInfo
{
    public string Name { get; set; } = null!;
    public DocumentCategoryEnum Category { get; set; }
    public string FileExtension { get; set; } = null!;
    public long SizeInBytes { get; set; }

    public string TemplateKey { get; set; } = string.Empty;

    public List<string> Tag { get; set; } = new List<string>();
    public string Description { get; set; } = null!;
    public string Url { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
}
