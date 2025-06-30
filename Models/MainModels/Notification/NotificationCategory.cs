namespace portal.Models;

public class NotificationCategory : BaseModelWithOnlyId
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public bool IsUrgentByDefault { get; set; } = false;
    public List<Notification> Notifications { get; set; } = new List<Notification>();
    public List<OnlyForOrganizationEntity> OnlyForOrganizationEntities { get; set; } =
        new List<OnlyForOrganizationEntity>();
}

// public List<AllowedRole> AllowedRoles { get; set; } = new List<AllowedRole>();

// public class AllowedRole : BaseModelWithOnlyId
// {
//     public long NotificationCategoryId { get; set; }
//     public NotificationCategory NotificationCategory { get; set; } = null!;
//     public int RoleId { get; set; }
//     public Role Role { get; set; } = null!;
// }
