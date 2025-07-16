namespace portal.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

public class StockConfiguration : BaseModelConfiguration<Stock>
{
    public override void Configure(EntityTypeBuilder<Stock> builder)
    {
        base.Configure(builder);

        builder.ToTable("Stocks");

        builder.HasIndex(s => new { s.MaterialId, s.WarehouseId }).IsUnique();

        builder.Property(s => s.TotalQuantity).IsRequired();
        builder.Property(s => s.ReservedQuantity);

        builder
            .HasOne(s => s.Material)
            .WithMany()
            .HasForeignKey(s => s.MaterialId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(s => s.Warehouse)
            .WithMany()
            .HasForeignKey(s => s.WarehouseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
