using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

namespace portal.Configuration.Models;

public class EmergencyContactInfoConfiguration
    : BaseModelWithOnlyIdConfiguration<EmergencyContactInfo>, // inherit your generic config
        IEntityTypeConfiguration<EmergencyContactInfo>
{
    public override void Configure(EntityTypeBuilder<EmergencyContactInfo> builder)
    {
        base.Configure(builder);

        builder.ToTable("EmergencyContactInfo");

        builder
            .HasOne(e => e.Employee)
            .WithMany(e => e.EmergencyContactInfos)
            .HasForeignKey(e => e.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Property configurations
        builder.Property(e => e.Name).HasMaxLength(100);

        builder.Property(e => e.Phone).HasMaxLength(20);

        builder.Property(e => e.Relationship).HasMaxLength(50);

        builder.Property(e => e.CurrentAddress).HasMaxLength(250);
    }
}
