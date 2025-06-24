using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace portal.Configuration.Models;

public class OrganizationEntityEmployeeConfiguration
    : IEntityTypeConfiguration<OrganizationEntityEmployee>
{
    public void Configure(EntityTypeBuilder<OrganizationEntityEmployee> builder)
    {
        // Composite PK on OrganizationEntityId + EmployeeId
        builder.HasKey(oee => new { oee.OrganizationEntityId, oee.EmployeeId });

        // FK → OrganizationEntity
        builder
            .HasOne(oee => oee.OrganizationEntity)
            .WithMany(oe => oe.OrganizationEntityEmployees)
            .HasForeignKey(oee => oee.OrganizationEntityId)
            .OnDelete(DeleteBehavior.Cascade);

        // FK → Employee
        builder
            .HasOne(oee => oee.Employee)
            .WithMany(e => e.OrganizationEntityEmployees)
            .HasForeignKey(oee => oee.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
