using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

namespace portal.Configuration.Models;

public class EmployeeQualificationInfoConfiguration
    : BaseModelWithOnlyIdConfiguration<EmployeeQualificationInfo>, // inherit your generic config
        IEntityTypeConfiguration<EmployeeQualificationInfo>
{
    public override void Configure(EntityTypeBuilder<EmployeeQualificationInfo> builder)
    {
        builder.ToTable("EmployeeQualificationInfo");

       base.Configure(builder);


        builder.HasOne(q => q.Employee)
               .WithMany(e => e.EmployeeQualificationInfos)
               .HasForeignKey(q => q.EmployeeId)
               .OnDelete(DeleteBehavior.Cascade);

        // Required fields
        builder.Property(q => q.Type)
               .IsRequired()
               .HasConversion<string>() // Store enum as string
               .HasMaxLength(30);

        builder.Property(q => q.Name)
               .IsRequired()
               .HasMaxLength(150);

        // Optional fields
        builder.Property(q => q.Institution)
               .HasMaxLength(100);

        builder.Property(q => q.GraduationOrIssueDate);
        builder.Property(q => q.ExpirationDate);

        builder.Property(q => q.CertificateNumber)
               .HasMaxLength(50);

        builder.Property(q => q.FileUrl)
               .HasMaxLength(300);

        builder.Property(q => q.Note)
               .HasMaxLength(500);
    }
}
