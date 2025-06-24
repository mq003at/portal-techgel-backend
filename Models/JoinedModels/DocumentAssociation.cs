using System.ComponentModel.DataAnnotations;
using portal.Models;

// Universal document association model for various places: workflows, dms, etc.
public class DocumentAssociation : BaseModelWithOnlyId
{
    [Required]
    public int DocumentId { get; set; }
    public Document Document { get; set; } = null!;

    // Which functionalities need this document, aka name of the model
    [Required]
    public string EntityType { get; set; } = null!;
    public int NodeId { get; set; }
}

