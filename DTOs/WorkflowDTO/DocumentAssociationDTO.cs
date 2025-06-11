using portal.DTOs;

public class DocumentAssociationDTO : BaseModelWithOnlyIdDTO
{
    public int DocumentId { get; set; }
    public string DocumentName { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;

    // List of associated entity IDs for polymorphic link
    public int EntityId { get; set; }
    public string DocumentURL { get; set; } = string.Empty;
}

public class DocumentAssociationCreateDTO : BaseModelWithOnlyIdCreateDTO
{
    public int DocumentId { get; set; }
    public string EntityType { get; set; } = string.Empty;
    public int EntityId { get; set; }
}

public class DocumentAssociationUpdateDTO : BaseModelWithOnlyIdUpdateDTO
{
    public int DocumentId { get; set; }
    public string EntityType { get; set; } = string.Empty;
    public int EntityId { get; set; }
}