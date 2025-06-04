namespace portal.Models;

public class Document : BaseModel
{
    // public string RemoteFileName { get; set; } = string.Empty;
    public GeneralDocumentInfo GeneralDocumentInfo { get; set; } = new();
    public LegalDocumentInfo LegalDocumentInfo { get; set; } = new();
    public SecurityDocumentInfo SecurityDocumentInfo { get; set; } = new();
    public AdditionalDocumentInfo AdditionalDocumentInfo { get; set; } = new();

    // History of edits
    public List<EditDocumentInfo> EditDocumentInfo { get; set; } =
        new List<EditDocumentInfo>();
}
