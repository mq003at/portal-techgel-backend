namespace portal.DTOs;

public class NotificationCategoryDTO : BaseModelWithOnlyIdDTO
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool IsUrgentByDefault { get; set; }
    public List<NotificationDTO> Notifications { get; set; } = new();
    public List<OrganizationEntityDTO> OnlyForOrganizationEntities { get; set; } = new();
}

public class NotificationCategoryCreateDTO : BaseModelWithOnlyIdCreateDTO
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool IsUrgentByDefault { get; set; }
    public List<long> OnlyForOrganizationEntityIds { get; set; } = new();
}

public class NotificationCategoryUpdateDTO : BaseModelWithOnlyIdUpdateDTO
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? IsUrgentByDefault { get; set; }
    public List<long>? OnlyForOrganizationEntityIds { get; set; }
}
