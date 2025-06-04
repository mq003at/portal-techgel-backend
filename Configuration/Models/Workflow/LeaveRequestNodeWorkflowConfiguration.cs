using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

namespace portal.Configuration;

public class LeaveRequestNodeConfiguration : BaseModelConfiguration<LeaveRequestNode>
{
    public override void Configure(EntityTypeBuilder<LeaveRequestNode> builder)
    {
        base.Configure(builder);

        builder.ToTable("LeaveRequestNodes");

        builder.Property(n => n.Name).HasMaxLength(255).IsRequired();
        builder.Property(n => n.Description).HasMaxLength(1000);
        builder.Property(n => n.Status).HasConversion<int>().IsRequired();
        builder.Property(n => n.SenderId).IsRequired();

        builder.Property(n => n.ApprovedByIds)
            .HasConversion(
                v => string.Join(",", v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList()
            );

        builder.Property(n => n.HasBeenApprovedByIds)
            .HasConversion(
                v => string.Join(",", v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList()
            );

        builder.Property(n => n.ApprovedDates)
            .HasConversion(
                v => string.Join(",", v.Select(d => d.ToString("o"))),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(DateTime.Parse).ToList()
            );

        builder.Property(n => n.DocumentIds)
            .HasConversion(
                v => string.Join(",", v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList()
            );

        builder.Property(n => n.StepType)
               .HasConversion<int>()
               .IsRequired();

    }
}
