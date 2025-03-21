using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.MainID).IsRequired(false);

        builder.HasIndex(d => d.Name).IsUnique();

        builder
            .HasOne(d => d.Division)
            .WithMany(div => div.Departments)
            .HasForeignKey(d => d.DivisionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(d => d.Sections)
            .WithOne(s => s.Department)
            .HasForeignKey(s => s.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
