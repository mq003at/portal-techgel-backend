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

        builder.Property(e => e.MainId).IsRequired().HasMaxLength(50);
        builder.HasIndex(e => e.MainId).IsUnique();

        builder.Property(e => e.ChildrenIds).HasColumnType("integer[]");

        builder.Property(e => e.Level).IsRequired();

        builder.Property(e => e.Name).IsRequired().HasMaxLength(200);

        builder.Property(e => e.Description).IsRequired().HasMaxLength(1000);

        builder.Property(e => e.Status).HasConversion<string>().HasMaxLength(20).IsRequired();

        builder.Property(e => e.SortOrder).HasDefaultValue(0);

        // Self-referencing relationship for hierarchy
        builder
            .HasOne(e => e.Parent)
            .WithMany(e => e.Children)
            .HasForeignKey(e => e.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Many-to-many relationship: OrganizationEntity ↔ Employees (through OrganizationEntityEmployee)
        builder
            .HasMany(e => e.OrganizationEntityEmployees)
            .WithOne(oe => oe.OrganizationEntity)
            .HasForeignKey(oe => oe.OrganizationEntityId)
            .OnDelete(DeleteBehavior.Cascade); // If an org is deleted, clean up join table

        // Direct relationship: Manager
        builder
            .HasOne(e => e.Manager)
            .WithMany()
            .HasForeignKey(e => e.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Direct relationship: Deputy Manager
        builder
            .HasOne(e => e.DeputyManager)
            .WithMany()
            .HasForeignKey(e => e.DeputyManagerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
