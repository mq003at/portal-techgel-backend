using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

namespace portal.Configuration.Models;

public class PersonalInfoConfiguration
    : BaseModelWithOnlyIdConfiguration<PersonalInfo>, // inherit your generic config
        IEntityTypeConfiguration<PersonalInfo>
{
    public override void Configure(EntityTypeBuilder<PersonalInfo> builder)
    {
        builder.ToTable("PersonalInfo");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired()
            .ValueGeneratedNever();

        builder.HasOne(p => p.Employee)
               .WithOne(e => e.PersonalInfo)
               .HasForeignKey<PersonalInfo>(p => p.EmployeeId)
               .OnDelete(DeleteBehavior.Cascade);

        // Required fields
        builder.Property(p => p.Gender)
               .IsRequired()
               .HasConversion<string>();

        builder.Property(p => p.MaritalStatus)
               .IsRequired()
               .HasConversion<string>();

        builder.Property(p => p.Address)
               .IsRequired()
               .HasMaxLength(250);

        builder.Property(p => p.Nationality)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(p => p.Birthplace)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(p => p.EthnicGroup)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(p => p.DateOfBirth)
               .IsRequired();

        // Optional fields
        builder.Property(p => p.PersonalEmail)
               .HasMaxLength(100);

        builder.Property(p => p.PersonalPhoneNumber)
               .HasMaxLength(20);

        builder.Property(p => p.IdCardNumber)
               .HasMaxLength(20);

        builder.Property(p => p.IdCardIssueDate);
        builder.Property(p => p.IdCardExpiryDate);
        builder.Property(p => p.IdCardIssuePlace)
               .HasMaxLength(100);
        }
}