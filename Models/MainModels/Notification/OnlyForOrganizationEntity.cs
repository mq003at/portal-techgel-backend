namespace portal.Models;

public class OnlyForOrganizationEntity : BaseModelWithOnlyId
{
    public int NotificationCategoryId { get; set; }
    public NotificationCategory NotificationCategory { get; set; } = null!;
    public int OrganizationEntityId { get; set; }
    public OrganizationEntity OrganizationEntity { get; set; } = null!;
}
