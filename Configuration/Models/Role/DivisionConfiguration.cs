using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

public class DivisionConfiguration : IEntityTypeConfiguration<Division>
{
    public void Configure(EntityTypeBuilder<Division> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.MainID).IsRequired(false);

        builder.HasIndex(d => d.Name).IsUnique();

        builder
            .HasMany(d => d.Departments)
            .WithOne(dep => dep.Division)
            .HasForeignKey(dep => dep.DivisionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
