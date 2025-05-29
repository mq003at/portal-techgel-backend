using Microsoft.EntityFrameworkCore;

namespace portal.Models;

[Owned]
public class LegalDocumentInfo
{
    public string DraftDate { get; set; } = string.Empty;
    public string PublishDate { get; set; } = string.Empty;
    public string EffectiveDate { get; set; } = string.Empty;
    public string ExpiredDate { get; set; } = string.Empty;
    public string FinalApprovalDate { get; set; } = string.Empty;
    public string InspectionDate { get; set; } = string.Empty;

    public ICollection<int> DraftByIds { get; set; } = new List<int>();
    public ICollection<int> PublishByIds { get; set; } = new List<int>();
    public ICollection<int> ApprovalByIds { get; set; } = new List<int>();
    public ICollection<int> InspectionByIds { get; set; } = new List<int>();

    public ICollection<string> DraftByNames { get; set; } = new List<string>();
    public ICollection<string> PublishByNames { get; set; } = new List<string>();
    public ICollection<string> ApprovalByNames { get; set; } = new List<string>();
    public ICollection<string> InspectionByNames { get; set; } = new List<string>();

    public bool IsLegalDocument { get; set; }
}
