using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

namespace portal.Configuration.Models;

public class InsuranceInfoConfiguration
    : BaseModelWithOnlyIdConfiguration<InsuranceInfo>, // inherit your generic config
        IEntityTypeConfiguration<InsuranceInfo>
{
    public override void Configure(EntityTypeBuilder<InsuranceInfo> builder)
    {
        builder.ToTable("InsuranceInfo");

       base.Configure(builder);


        builder.HasOne(x => x.Employee)
               .WithOne(e => e.InsuranceInfo)
               .HasForeignKey<InsuranceInfo>(x => x.EmployeeId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.Employee)
               .WithOne(e => e.InsuranceInfo)
               .HasForeignKey<InsuranceInfo>(i => i.EmployeeId)
               .OnDelete(DeleteBehavior.Cascade); // Delete insurance info if employee is deleted

        // Required fields
        builder.Property(i => i.InsuranceCode)
               .IsRequired()
               .HasMaxLength(50); // Optional: restrict to prevent overly long values

        builder.Property(i => i.RegistrationLocation)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(i => i.EffectiveDate)
               .IsRequired();

        builder.Property(i => i.InsuranceStatus)
               .IsRequired()
               .HasMaxLength(30);

        // Optional fields
        builder.Property(i => i.TerminationDate);
        builder.Property(i => i.Note)
               .HasMaxLength(500); // prevent large blobs
    }
}
