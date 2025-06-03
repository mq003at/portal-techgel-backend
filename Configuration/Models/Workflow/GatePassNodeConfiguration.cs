using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace portal.Models;

public class GatePassNodesConfiguration : BaseModelConfiguration<GatePassNodes>
{
    public override void Configure(EntityTypeBuilder<GatePassNodes> builder)
    {
        base.Configure(builder);

        builder.ToTable("GatePassNodes");

        builder.Property(n => n.Name).HasMaxLength(255).IsRequired();
        builder.Property(n => n.Description).HasMaxLength(1000);

        builder.Property(n => n.Status)
               .HasConversion<int>()
               .IsRequired();

        builder.Property(n => n.SenderId).IsRequired();

        builder.Property(n => n.ApprovedByIds)
               .HasConversion(
                   v => string.Join(",", v),
                   v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList()
               );

        builder.Property(n => n.GatePassId).IsRequired();
    }
}