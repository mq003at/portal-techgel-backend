using portal.DTOs;

public class OnlyForOrganizationEntityDTO : BaseModelWithOnlyIdDTO
{
    public long NotificationCategoryId { get; set; }
    public string NotificationCategoryName { get; set; } = string.Empty;

    public int OrganizationEntityId { get; set; }
    public string OrganizationEntityName { get; set; } = string.Empty;
}

public class OnlyForOrganizationEntityCreateDTO : BaseModelWithOnlyIdCreateDTO
{
    public long NotificationCategoryId { get; set; }
    public int OrganizationEntityId { get; set; }
}

public class OnlyForOrganizationEntityUpdateDTO : BaseModelWithOnlyIdUpdateDTO
{
    public long NotificationCategoryId { get; set; }
    public int OrganizationEntityId { get; set; }
}
