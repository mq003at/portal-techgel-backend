using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

public class OnlyForOrganizationEntityConfiguration
    : BaseModelWithOnlyIdConfiguration<OnlyForOrganizationEntity>
{
    public override void Configure(EntityTypeBuilder<OnlyForOrganizationEntity> builder)
    {
        base.Configure(builder);

        // Required foreign key to NotificationCategory
        builder.HasOne(e => e.NotificationCategory)
            .WithMany(c => c.OnlyForOrganizationEntities)
            .HasForeignKey(e => e.NotificationCategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // Required foreign key to OrganizationEntity
        builder.HasOne(e => e.OrganizationEntity)
            .WithMany()
            .HasForeignKey(e => e.OrganizationEntityId)
            .OnDelete(DeleteBehavior.Cascade);

        // Optional: prevent duplicates (unique combination)
        builder.HasIndex(e => new { e.NotificationCategoryId, e.OrganizationEntityId })
            .IsUnique();
    }
}
