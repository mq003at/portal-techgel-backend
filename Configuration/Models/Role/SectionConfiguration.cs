using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

public class SectionConfiguration : IEntityTypeConfiguration<Section>
{
    public void Configure(EntityTypeBuilder<Section> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.MainID).IsRequired(false);

        builder.HasIndex(s => s.Name).IsUnique();

        builder
            .HasOne(s => s.Department)
            .WithMany(dep => dep.Sections)
            .HasForeignKey(s => s.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(s => s.Units)
            .WithOne(u => u.Section)
            .HasForeignKey(u => u.SectionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
