using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

namespace portal.Configuration.Models;

public class SignatureConfiguration
    : BaseModelConfiguration<Signature>,
        IEntityTypeConfiguration<Signature>
{
    public override void Configure(EntityTypeBuilder<Signature> builder)
    {
        base.Configure(builder);
        builder.Property(s => s.EmployeeId).IsRequired();

        builder.Property(s => s.FileName).IsRequired().HasMaxLength(255);

        builder.Property(s => s.ContentType).IsRequired().HasMaxLength(100);

        builder.Property(s => s.FileSize).IsRequired();

        builder.Property(s => s.StoragePath).IsRequired().HasMaxLength(500);

        builder
            .Property(s => s.UploadedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        builder
            .HasOne(s => s.Employee)
            .WithOne(e => e.Signature)
            .HasForeignKey<Signature>(s => s.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
