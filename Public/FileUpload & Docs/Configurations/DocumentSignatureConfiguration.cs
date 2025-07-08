
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

        builder.HasOne(ds => ds.Employee)
            .WithMany() 
            .HasForeignKey(ds => ds.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}