using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace portal.Configuration.Models;

public class OrganizationEntityEmployeeConfiguration
    : IEntityTypeConfiguration<OrganizationEntityEmployee>
{
    public void Configure(EntityTypeBuilder<OrganizationEntityEmployee> builder)
    {
        builder.HasKey(oe => new { oe.OrganizationEntityId, oe.EmployeeId });

        builder
            .HasOne(oe => oe.OrganizationEntity)
            .WithMany(o => o.OrganizationEntityEmployees)
            .HasForeignKey(oe => oe.OrganizationEntityId);

        builder
            .HasOne(oe => oe.Employee)
            .WithMany(e => e.OrganizationEntityEmployees)
            .HasForeignKey(oe => oe.EmployeeId);
    }
}
