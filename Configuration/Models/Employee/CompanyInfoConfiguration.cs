using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

namespace portal.Configuration.Models;

public class CompanyInfoConfiguration
    : BaseModelWithOnlyIdConfiguration<CompanyInfo>, // inherit your generic config
        IEntityTypeConfiguration<CompanyInfo>
{
    public override void Configure(EntityTypeBuilder<CompanyInfo> builder)
    {       base.Configure(builder);

        builder.ToTable("CompanyInfo");


         builder.HasOne(c => c.Employee)
               .WithOne(e => e.CompanyInfo)
               .HasForeignKey<CompanyInfo>(c => c.EmployeeId)
               .OnDelete(DeleteBehavior.Cascade);

        // Email & Phone
        builder.Property(c => c.CompanyEmail)
               .HasMaxLength(100);

        builder.Property(c => c.CompanyPhoneNumber)
               .HasMaxLength(20);

        // Employment Status (Enum to string)
        builder.Property(c => c.EmploymentStatus)
               .HasConversion<string>()
               .IsRequired();

        builder.Property(c => c.Position)
               .HasMaxLength(100);

        builder.Property(c => c.Department)
               .HasMaxLength(100);

        // Dates
        builder.Property(c => c.StartDate);
        builder.Property(c => c.EndDate);
        builder.Property(c => c.ProbationStartDate);
        builder.Property(c => c.ProbationEndDate);

        // Booleans and numbers
        builder.Property(c => c.IsOnProbation)
               .IsRequired();

        builder.Property(c => c.CompensatoryLeaveTotalDays)
               .IsRequired()
               .HasDefaultValue(0.0);

        builder.Property(c => c.AnnualLeaveTotalDays)
               .IsRequired()
               .HasDefaultValue(0.0);
        }
}