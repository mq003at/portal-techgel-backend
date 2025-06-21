using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

namespace portal.Configuration.Models;

public class TaxInfoConfiguration
    : BaseModelWithOnlyIdConfiguration<TaxInfo>, // inherit your generic config
        IEntityTypeConfiguration<TaxInfo>
{
    public override void Configure(EntityTypeBuilder<TaxInfo> builder)
    {
        builder.ToTable("TaxInfo");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired()
            .ValueGeneratedNever();

        builder.HasOne(x => x.Employee)
               .WithOne(e => e.TaxInfo)
               .HasForeignKey<TaxInfo>(x => x.EmployeeId)
               .OnDelete(DeleteBehavior.Restrict);

        // One-to-one: TaxInfo ↔ Supervisor (another employee)
        builder.Property(t => t.TaxCode)
               .IsRequired()
               .HasMaxLength(20); // assuming MST won't exceed 20 chars

        // Optional fields
        builder.Property(t => t.RegistrationDate);

        builder.Property(t => t.NumberOfDependents)
               .IsRequired()
               .HasDefaultValue(0);

        builder.Property(t => t.IsFamilyStatusRegistered)
               .IsRequired()
               .HasDefaultValue(false);

        builder.Property(t => t.Note)
               .HasMaxLength(500);
    }
}