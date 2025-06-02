using Microsoft.EntityFrameworkCore;

namespace portal.Models;

public class EditDocumentInfo
{
    public int EditorId { get; set; }
    public string EditorName { get; set; } = string.Empty;
    public DateTime EditDate { get; set; }
    public string Comments { get; set; } = string.Empty;
}
