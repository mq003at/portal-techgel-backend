using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

namespace portal.Configuration;

public class NotificationCategoryConfiguration : BaseModelWithOnlyIdConfiguration<NotificationCategory>
{
    public override void Configure(EntityTypeBuilder<NotificationCategory> builder)
    {
        base.Configure(builder);

        builder.Property(n => n.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(n => n.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(n => n.IsUrgentByDefault)
            .IsRequired();

        builder.HasMany(n => n.Notifications)
            .WithOne(n => n.NotificationCategory)
            .HasForeignKey(n => n.NotificationCategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(n => n.OnlyForOrganizationEntities)
            .WithOne(o => o.NotificationCategory)
            .HasForeignKey(o => o.NotificationCategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}