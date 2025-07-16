namespace portal.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

public class MaterialConfiguration : BaseModelConfiguration<Material>
{
    public override void Configure(EntityTypeBuilder<Material> builder)
    {
        base.Configure(builder);

        builder.ToTable("Materials");

        builder.Property(m => m.Code).IsRequired().HasMaxLength(100);

        builder.HasIndex(m => m.Code).IsUnique();

        builder.Property(m => m.Name).IsRequired().HasMaxLength(200);

        builder.Property(m => m.Unit).IsRequired().HasMaxLength(50);

        builder.Property(m => m.Type).IsRequired();

        builder.Property(m => m.Brand).HasMaxLength(100);

        builder.Property(m => m.Specification).HasMaxLength(500);

        builder.Property(m => m.IsHazardous).HasDefaultValue(false);
    }
}
