using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

public class UnitConfiguration : IEntityTypeConfiguration<Unit>
{
    public void Configure(EntityTypeBuilder<Unit> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.MainID).IsRequired(false);

        builder.HasIndex(u => u.Name).IsUnique();

        builder
            .HasOne(u => u.Section)
            .WithMany(s => s.Units)
            .HasForeignKey(u => u.SectionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(u => u.Teams)
            .WithOne(t => t.Unit)
            .HasForeignKey(t => t.UnitId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
