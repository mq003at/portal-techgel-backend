using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace portal.Configuration;

public class NotificationConfiguration : BaseModelConfiguration<Notification>
{
    public override void Configure(EntityTypeBuilder<Notification> builder)
    {
        base.Configure(builder);

        builder.Property(n => n.Title)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(n => n.Message)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(n => n.Url)
            .HasMaxLength(500);

        builder.Property(n => n.ReadAt)
            .HasColumnType("timestamp with time zone"); // PostgreSQL friendly

        builder.Property(n => n.FinishedAt)
            .HasColumnType("timestamp with time zone");

        builder.Property(n => n.UrgencyLevel)
            .HasConversion<string>() // Store as string, not int
            .HasMaxLength(50);

        builder.HasOne(n => n.Employee)
            .WithMany()
            .HasForeignKey(n => n.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(n => n.NotificationCategory)
            .WithMany(c => c.Notifications)
            .HasForeignKey(n => n.NotificationCategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}