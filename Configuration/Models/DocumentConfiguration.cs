using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Enums;
using portal.Models;

namespace portal.Configuration;

public class DocumentConfiguration
    : BaseModelConfiguration<Document>,
        IEntityTypeConfiguration<Document>
{
    public override void Configure(EntityTypeBuilder<Document> builder)
    {
        base.Configure(builder);

        builder.ToTable("Documents");
        builder.HasKey(d => d.Id);

        builder.OwnsOne(
            d => d.GeneralDocumentInfo,
            g =>
            {
                g.Property(x => x.Name).IsRequired().HasMaxLength(255);
                g.Property(x => x.Type).IsRequired();
                g.Property(x => x.Status).IsRequired();
                g.Property(x => x.SubType).IsRequired();
                g.Property(x => x.Category).IsRequired();

                g.Property(x => x.OwnerId).IsRequired();
                g.Property(x => x.OwnerName).HasMaxLength(255);

                g.Property(x => x.OrganizationEntityResponsibleId).IsRequired();
                g.Property(x => x.OrganizationEntityResponsibleName).HasMaxLength(255);

                g.Property(x => x.Description).HasMaxLength(1000).HasDefaultValue(string.Empty);
                g.Property(x => x.Url).HasMaxLength(500).HasDefaultValue(string.Empty);
                g.Property(x => x.Version).HasMaxLength(50).HasDefaultValue(string.Empty);

                g.Ignore(x => x.Tag);
            }
        );

        builder.OwnsOne(
     d => d.LegalDocumentInfo,
     l =>
     {
         // Date string properties
         l.Property(x => x.DraftDate).HasMaxLength(500).HasDefaultValue(string.Empty);
         l.Property(x => x.PublishDate).HasMaxLength(500).HasDefaultValue(string.Empty);
         l.Property(x => x.EffectiveDate).HasMaxLength(500).HasDefaultValue(string.Empty);
         l.Property(x => x.ExpiredDate).HasMaxLength(500).HasDefaultValue(string.Empty);
         l.Property(x => x.FinalApprovalDate).HasMaxLength(500).HasDefaultValue(string.Empty);
         l.Property(x => x.InspectionDate).HasMaxLength(500).HasDefaultValue(string.Empty);

         // Persist int collections as JSON columns
         l.Property(x => x.DraftByIds)
             .HasConversion(
                 v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                 v => string.IsNullOrWhiteSpace(v)
                     ? new List<int>()
                     : JsonSerializer.Deserialize<List<int>>(v, (JsonSerializerOptions?)null) ?? new List<int>()
             );

         l.Property(x => x.PublishByIds)
             .HasConversion(
                 v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                 v => string.IsNullOrWhiteSpace(v)
                     ? new List<int>()
                     : JsonSerializer.Deserialize<List<int>>(v, (JsonSerializerOptions?)null) ?? new List<int>()
             );

         l.Property(x => x.HaveApprovedByIds)
             .HasConversion(
                 v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                 v => string.IsNullOrWhiteSpace(v)
                     ? new List<int>()
                     : JsonSerializer.Deserialize<List<int>>(v, (JsonSerializerOptions?)null) ?? new List<int>()
             );

         l.Property(x => x.InspectionByIds)
             .HasConversion(
                 v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                 v => string.IsNullOrWhiteSpace(v)
                     ? new List<int>()
                     : JsonSerializer.Deserialize<List<int>>(v, (JsonSerializerOptions?)null) ?? new List<int>()
             );

         l.Property(x => x.RequestApprovalByIds)
             .HasConversion(
                 v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                 v => string.IsNullOrWhiteSpace(v)
                     ? new List<int>()
                     : JsonSerializer.Deserialize<List<int>>(v, (JsonSerializerOptions?)null) ?? new List<int>()
             );

         l.Property(x => x.DocumentApprovalLogic).IsRequired();

         // Ignore all name collections (they are computed, not stored)
         l.Ignore(x => x.DraftByNames);
         l.Ignore(x => x.PublishByNames);
         l.Ignore(x => x.ApprovalByNames);
         l.Ignore(x => x.InspectionByNames);
         l.Ignore(x => x.HaveApprovedByNames);
         l.Ignore(x => x.RequestApprovalByNames);

         // Legal document flag
         l.Property(x => x.IsLegalDocument).HasDefaultValue(false);
     }
 );


        builder.Ignore(d => d.SecurityDocumentInfo);
        builder.Ignore(d => d.AdditionalDocumentInfo);
        builder.Ignore(d => d.EditDocumentInfo);
    }
}
