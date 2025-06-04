using Microsoft.EntityFrameworkCore;
using portal.Enums;

namespace portal.Models;

public class LegalDocumentInfo
{
    public string DraftDate { get; set; } = string.Empty;
    public string PublishDate { get; set; } = string.Empty;
    public string EffectiveDate { get; set; } = string.Empty;
    public string ExpiredDate { get; set; } = string.Empty;
    public string FinalApprovalDate { get; set; } = string.Empty;
    public string InspectionDate { get; set; } = string.Empty;

    public List<int> DraftByIds { get; set; } = new List<int>();
    public List<int> PublishByIds { get; set; } = new List<int>();
    public List<int> HaveApprovedByIds { get; set; } = new List<int>();
    public List<int> InspectionByIds { get; set; } = new List<int>();
    public List<int> RequestApprovalByIds { get; set; } = new List<int>();
    public DocumentApprovalLogicEnum DocumentApprovalLogic { get; set; } = DocumentApprovalLogicEnum.PARALLEL;

    public List<string> DraftByNames { get; set; } = new List<string>();
    public List<string> PublishByNames { get; set; } = new List<string>();
    public List<string> ApprovalByNames { get; set; } = new List<string>();
    public List<string> InspectionByNames { get; set; } = new List<string>();

    public List<string> HaveApprovedByNames { get; set; } = new List<string>();
    public List<string> RequestApprovalByNames { get; set; } = new List<string>();
    public bool IsLegalDocument { get; set; }
}
