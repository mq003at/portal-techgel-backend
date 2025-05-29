using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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
                l.Property(x => x.DraftDate).HasColumnType("date");
                l.Property(x => x.PublishDate).HasColumnType("date");
                l.Property(x => x.EffectiveDate).HasColumnType("date");
                l.Property(x => x.ExpiredDate).HasColumnType("date");
                l.Property(x => x.FinalApprovalDate).HasColumnType("date");
                l.Property(x => x.InspectionDate).HasColumnType("date");

                l.Ignore(x => x.DraftByIds);
                l.Ignore(x => x.PublishByIds);
                l.Ignore(x => x.ApprovalByIds);
                l.Ignore(x => x.InspectionByIds);
                l.Ignore(x => x.DraftByNames);
                l.Ignore(x => x.PublishByNames);
                l.Ignore(x => x.ApprovalByNames);
                l.Ignore(x => x.InspectionByNames);

                l.Property(x => x.IsLegalDocument).HasDefaultValue(false);
            }
        );

        builder.Ignore(d => d.SecurityDocumentInfo);
        builder.Ignore(d => d.AdditionalDocumentInfo);
        builder.Ignore(d => d.EditDocumentInfo);
    }
}
