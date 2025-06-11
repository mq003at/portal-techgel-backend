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
        builder.HasOne(e => e.Document)
            .WithMany()
            .HasForeignKey(e => e.DocumentId)
            .OnDelete(DeleteBehavior.Cascade); 

        builder.Property(e => e.EntityType)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.EntityId).IsRequired();
      }
}