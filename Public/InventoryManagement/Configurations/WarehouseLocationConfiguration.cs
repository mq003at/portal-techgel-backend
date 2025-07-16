namespace portal.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

public class WarehouseLocationConfiguration : BaseModelConfiguration<WarehouseLocation>
{
    public override void Configure(EntityTypeBuilder<WarehouseLocation> builder)
    {
        base.Configure(builder);

        builder.ToTable("WarehouseLocations");

        builder.Property(wl => wl.Zone).IsRequired().HasMaxLength(10);

        builder.Property(wl => wl.Aisle).IsRequired();

        builder.Property(wl => wl.Rack).IsRequired();

        builder.Property(wl => wl.Shelf).IsRequired().HasMaxLength(10);

        builder
            .HasIndex(wl => new
            {
                wl.WarehouseId,
                wl.Zone,
                wl.Aisle,
                wl.Rack,
                wl.Shelf
            })
            .IsUnique();

        builder
            .HasOne(wl => wl.Warehouse)
            .WithMany()
            .HasForeignKey(wl => wl.WarehouseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
