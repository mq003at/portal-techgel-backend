using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

namespace portal.Configuration.Models;

public class OrganizationEntityConfiguration : BaseModelConfiguration<OrganizationEntity>
{
    public override void Configure(EntityTypeBuilder<OrganizationEntity> builder)
    {
        base.Configure(builder);

        builder.ToTable("OrganizationEntities");

        builder.Property(e => e.Level)
            .IsRequired();

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(e => e.Status)
            .IsRequired();

        builder.Property(e => e.SortOrder)
            .HasDefaultValue(0);

        // Self-referencing relationship for hierarchy
        builder.HasOne(e => e.Parent)
            .WithMany(e => e.Children)
            .HasForeignKey(e => e.ParentId)
            .OnDelete(DeleteBehavior.Restrict); // Avoid cascade delete in hierarchy

        // Many-to-many via linking table
        builder
            .HasMany(e => e.OrganizationEntityEmployees)
            .WithOne(oe => oe.OrganizationEntity)
            .HasForeignKey(oe => oe.OrganizationEntityId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
