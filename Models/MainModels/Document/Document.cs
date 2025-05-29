namespace portal.Models;

public class Document : BaseModel
{
    public GeneralDocumentInfo GeneralDocumentInfo { get; set; } = new();
    public LegalDocumentInfo LegalDocumentInfo { get; set; } = new();
    public SecurityDocumentInfo SecurityDocumentInfo { get; set; } = new();
    public AdditionalDocumentInfo AdditionalDocumentInfo { get; set; } = new();

    // History of edits
    public ICollection<EditDocumentInfo> EditDocumentInfo { get; set; } =
        new List<EditDocumentInfo>();
}
