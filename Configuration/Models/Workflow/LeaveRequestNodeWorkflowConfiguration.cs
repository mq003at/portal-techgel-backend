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

        // KHÔNG CẦN HasConversion cho các mảng nữa!
        builder.Property(n => n.ApprovedByIds).HasColumnType("integer[]");
        builder.Property(n => n.HasBeenApprovedByIds).HasColumnType("integer[]");
        builder.Property(n => n.DocumentIds).HasColumnType("integer[]");
        builder.Property(n => n.ApprovedDates).HasColumnType("timestamptz[]");

        builder.Property(n => n.StepType)
               .HasConversion<int>()
               .IsRequired();
    }
}
