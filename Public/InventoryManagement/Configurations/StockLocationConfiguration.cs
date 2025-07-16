namespace portal.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

public class StockLocationConfiguration : BaseModelConfiguration<StockLocation>
{
    public override void Configure(EntityTypeBuilder<StockLocation> builder)
    {
        base.Configure(builder);

        builder.ToTable("StockLocations");

        builder.Property(sl => sl.Quantity).IsRequired();

        builder.HasIndex(sl => new { sl.StockId, sl.WarehouseLocationId }).IsUnique();

        builder
            .HasOne(sl => sl.Stock)
            .WithMany(s => s.StockLocations)
            .HasForeignKey(sl => sl.StockId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(sl => sl.WarehouseLocation)
            .WithMany(wl => wl.StockLocations)
            .HasForeignKey(sl => sl.WarehouseLocationId)
            .OnDelete(DeleteBehavior.Restrict); // prevent cascading location deletion
    }
}
