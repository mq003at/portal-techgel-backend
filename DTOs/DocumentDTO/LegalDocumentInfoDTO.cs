namespace portal.DTOs;

public class LegalDocumentInfoDTO
{
    public string DraftDate { get; set; } = string.Empty; // dd/MM/yyyy
    public string PublishDate { get; set; } = string.Empty;
    public string EffectiveDate { get; set; } = string.Empty;
    public string ExpiredDate { get; set; } = string.Empty;
    public string FinalApprovalDate { get; set; } = string.Empty;
    public string InspectionDate { get; set; } = string.Empty;

    public List<int> DraftByIds { get; set; } = new();
    public List<int> PublishByIds { get; set; } = new();
    public List<int> ApprovalByIds { get; set; } = new();
    public List<int> InspectionByIds { get; set; } = new();

    public List<string> DraftByNames { get; set; } = new();
    public List<string> PublishByNames { get; set; } = new();
    public List<string> ApprovalByNames { get; set; } = new();
    public List<string> InspectionByNames { get; set; } = new();

    public bool IsLegalDocument { get; set; }
}

public class CreateLegalDocumentInfoDTO
{
    // Dates in dd/MM/yyyy format
    public string DraftDate { get; set; } = string.Empty;
    public string PublishDate { get; set; } = string.Empty;
    public string EffectiveDate { get; set; } = string.Empty;
    public string ExpiredDate { get; set; } = string.Empty;
    public string FinalApprovalDate { get; set; } = string.Empty;
    public string InspectionDate { get; set; } = string.Empty;

    public List<int> DraftByIds { get; set; } = new();
    public List<int> PublishByIds { get; set; } = new();
    public List<int> ApprovalByIds { get; set; } = new();
    public List<int> InspectionByIds { get; set; } = new();

    public bool IsLegalDocument { get; set; }
}

public class UpdateLegalDocumentInfoDTO
{
    public string? DraftDate { get; set; }
    public string? PublishDate { get; set; }
    public string? EffectiveDate { get; set; }
    public string? ExpiredDate { get; set; }
    public string? FinalApprovalDate { get; set; }
    public string? InspectionDate { get; set; }

    public List<int>? DraftByIds { get; set; }
    public List<int>? PublishByIds { get; set; }
    public List<int>? ApprovalByIds { get; set; }
    public List<int>? InspectionByIds { get; set; }

    public bool? IsLegalDocument { get; set; }
}