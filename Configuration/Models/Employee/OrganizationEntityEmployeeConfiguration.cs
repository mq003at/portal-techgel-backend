using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

namespace portal.Configuration.Models;

public class OrganizationEnittyEmployeeConfiguration
    : BaseModelConfiguration<OrganizationEntityEmployee>, // inherit your generic config
        IEntityTypeConfiguration<OrganizationEntityEmployee>
{
    public override void Configure(EntityTypeBuilder<OrganizationEntityEmployee> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.OrganizationEntity)
               .WithMany(o => o.OrganizationEntityEmployees)
               .HasForeignKey(x => x.OrganizationEntityId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.OrganizationRelationType)
         .HasConversion<string>()
            .IsRequired();

        builder.HasOne(x => x.Employee)
            .WithMany(e => e.OrganizationEntityEmployees)
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.IsPrimary)
            .HasDefaultValue(false);
    }
}