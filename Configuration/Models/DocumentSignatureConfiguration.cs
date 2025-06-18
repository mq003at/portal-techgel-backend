
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Enums;
using portal.Models;

namespace portal.Configuration;
public class DocumentSignatureConfiguration : IEntityTypeConfiguration<DocumentSignature>
{
    public void Configure(EntityTypeBuilder<DocumentSignature> builder)
    {
        builder.HasKey(ds => ds.Id);

        builder.Property(ds => ds.SignedAt)
            .IsRequired();

        builder.Property(ds => ds.SignatureUrl)
            .HasMaxLength(500)
            .IsRequired();

        builder.HasOne(ds => ds.Document)
            .WithMany(d => d.Signatures)
            .HasForeignKey(ds => ds.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ds => ds.Employee)
            .WithMany() // Optional: .WithMany(e => e.DocumentSignatures) if backref exists
            .HasForeignKey(ds => ds.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}