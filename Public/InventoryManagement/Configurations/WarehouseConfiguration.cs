namespace portal.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

public class WarehouseConfiguration : BaseModelConfiguration<Warehouse>
{
    public override void Configure(EntityTypeBuilder<Warehouse> builder)
    {
        base.Configure(builder);

        builder.ToTable("Warehouses");

        builder.Property(w => w.Name).IsRequired().HasMaxLength(150);

        builder.HasIndex(w => w.Name).IsUnique();

        builder.Property(w => w.Location).HasMaxLength(300);

        builder.Property(w => w.Capacity).IsRequired();

        builder.Property(w => w.IsProjectSite).HasDefaultValue(false);

        builder
            .HasOne(w => w.Manager)
            .WithMany()
            .HasForeignKey(w => w.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
