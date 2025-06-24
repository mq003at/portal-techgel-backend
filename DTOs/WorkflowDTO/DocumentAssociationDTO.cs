using portal.DTOs;

public class DocumentAssociationDTO : BaseModelWithOnlyIdDTO
{
    public int DocumentId { get; set; }
    public string DocumentName { get; set; } = string.Empty;
    public string NodeType { get; set; } = string.Empty;

    // List of associated entity IDs for polymorphic link
    public int NodeId { get; set; }
    
    public string DocumentURL { get; set; } = null!;
}

public class DocumentAssociationCreateDTO : BaseModelWithOnlyIdCreateDTO
{
    public int DocumentId { get; set; }
    public required string NodeType { get; set; }
    public int NodeId { get; set; }
}

public class DocumentAssociationUpdateDTO : BaseModelWithOnlyIdUpdateDTO
{
    public int? DocumentId { get; set; }
    public string? NodeType { get; set; } 
    public int? NodeId { get; set; }
}