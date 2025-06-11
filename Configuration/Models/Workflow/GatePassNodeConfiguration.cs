// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;

// namespace portal.Models;

// public class GatePassNodesConfiguration : BaseModelConfiguration<GatePassNodes>
// {
//     public override void Configure(EntityTypeBuilder<GatePassNodes> builder)
//     {
//         base.Configure(builder);

//         builder.ToTable("GatePassNodes");

//         builder.Property(n => n.Name)
//                .HasMaxLength(255)
//                .IsRequired();

//         builder.Property(n => n.Description)
//                .HasMaxLength(1000);

//         builder.Property(n => n.Status)
//                .HasConversion<int>()
//                .IsRequired();

//         builder.Property(n => n.SenderId).IsRequired();

//         builder.Property(n => n.ApprovedByIds)
//                .HasConversion(
//                    v => string.Join(",", v),
//                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList()
//                );

//         builder.Property(n => n.HasBeenApprovedByIds)
//                .HasConversion(
//                    v => string.Join(",", v),
//                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList()
//                );

//         builder.Property(n => n.ApprovedDates)
//                .HasConversion(
//                    v => string.Join(",", v.Select(d => d.ToString("o"))),
//                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(DateTime.Parse).ToList()
//                );

//         builder.Property(n => n.DocumentIds)
//                .HasConversion(
//                    v => string.Join(",", v),
//                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList()
//                );

//         // Relationship to GatePass
//         builder.HasOne(n => n.GatePass)
//                .WithMany(g => g.GatePassNodes)
//                .HasForeignKey(n => n.GatePassId)
//                .OnDelete(DeleteBehavior.Cascade);
//     }
// }
