using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

public class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.MainID).IsRequired(false);

        builder.HasIndex(t => t.Name).IsUnique();

        builder
            .HasOne(t => t.Unit)
            .WithMany(u => u.Teams)
            .HasForeignKey(t => t.UnitId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
