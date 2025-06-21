using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Enums;
using portal.Models;

namespace portal.Configuration;

public class DocumentConfiguration : BaseModelConfiguration<Document>
{
    public override void Configure(EntityTypeBuilder<Document> builder)
    {
        base.Configure(builder);

        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(d => d.Status)
            .IsRequired()
            .HasConversion<string>(); // Store enum as string

        builder.Property(d => d.Category)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(d => d.FileExtension)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(d => d.SizeInBytes)
            .IsRequired();

        builder.Property(d => d.TemplateKey)
            .HasMaxLength(255);

        builder.Property(d => d.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(d => d.Division)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.Url)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(d => d.Version)
            .IsRequired()
            .HasMaxLength(100);

        // Tags: stored as a delimited string, you may customize the separator
        builder.Property(d => d.Tag)
            .HasConversion(
                v => string.Join(";", v),
                v => v.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList()
            );       
    }
}