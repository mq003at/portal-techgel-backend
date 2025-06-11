// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;
// using portal.Models;

// namespace portal.Configuration;
// public class GatePassConfiguration : BaseModelConfiguration<GatePass>
// {
//     public override void Configure(EntityTypeBuilder<GatePass> builder)
//     {
//         base.Configure(builder);

//         builder.ToTable("GatePasses");

//         builder.Property(g => g.EmployeeId).IsRequired();
//         builder.Property(g => g.Reason).HasMaxLength(500);
//         builder.Property(g => g.StartDate).IsRequired();
//         builder.Property(g => g.EndDate).IsRequired();

//         builder.Property(g => g.Name).HasMaxLength(255).IsRequired();
//         builder.Property(g => g.Description).HasMaxLength(1000);

//         builder.Property(g => g.Status)
//                .HasConversion<int>()
//                .IsRequired();

//         builder.Property(g => g.ReceiverIds)
//                .HasConversion(
//                    v => string.Join(",", v),
//                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList()
//                );

//         builder.Property(g => g.DraftedByIds)
//                .HasConversion(
//                    v => string.Join(",", v),
//                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList()
//                );

//         builder.Property(g => g.HasBeenApprovedByIds)
//                .HasConversion(
//                    v => string.Join(",", v),
//                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList()
//                );

//         builder.Property(g => g.ApprovedDates)
//                .HasConversion(
//                    v => string.Join(",", v.Select(d => d.ToString("o"))),
//                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(DateTime.Parse).ToList()
//                );

//         builder.HasMany(g => g.GatePassNodes)
//                .WithOne(n => n.GatePass)
//                .HasForeignKey(n => n.GatePassId)
//                .OnDelete(DeleteBehavior.Cascade);
//     }
// }
