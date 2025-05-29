using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

namespace portal.Configuration.Models;

public class OrganizationEntityConfiguration
    : BaseModelConfiguration<OrganizationEntity>,
        IEntityTypeConfiguration<OrganizationEntity>
{
    public override void Configure(EntityTypeBuilder<OrganizationEntity> builder)
    {
        builder.HasKey(o => o.Id);

        // Parent ↔ Children
        builder.HasOne(o => o.Parent).WithMany(o => o.Children).HasForeignKey(o => o.ParentId);

        // Manager ↔ OrganizationEntity
        builder
            .HasOne(o => o.Manager)
            .WithMany() // nếu không cần navigation ngược
            .HasForeignKey(o => o.ManagerId);

        // Owned types nếu không tách riêng class
        // builder.OwnsOne(o => o.SomeOwnedType);
    }
}
