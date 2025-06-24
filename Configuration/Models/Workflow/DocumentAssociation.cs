using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class DocumentAssociationConfiguration : IEntityTypeConfiguration<DocumentAssociation>
{
    public void Configure(EntityTypeBuilder<DocumentAssociation> builder)
    {
        // Primary Key
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();

        // DocumentId: Required FK
        builder.Property(e => e.DocumentId).IsRequired();

        // Navigation to Document
        builder.HasOne(d => d.Document)
            .WithMany(d => d.DocumentAssociations)
            .HasForeignKey(d => d.DocumentId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.Property(e => e.EntityType)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(d => new { d.NodeId, d.EntityType });

        builder.Property(e => e.NodeId).IsRequired();
      }
}