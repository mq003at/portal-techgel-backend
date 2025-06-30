using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace portal.Configuration;

public class OrganizationEntityEmployeeConfiguration
    : BaseModelConfiguration<OrganizationEntityEmployee>,
        IEntityTypeConfiguration<OrganizationEntityEmployee>
{
    public override void Configure(EntityTypeBuilder<OrganizationEntityEmployee> builder)
    {
        // Apply base configuration (Id, MainId, timestamps)
        base.Configure(builder);

        // Foreign key to OrganizationEntity
        builder
            .HasOne(x => x.OrganizationEntity)
            .WithMany(x => x.OrganizationEntityEmployees)
            .HasForeignKey(x => x.OrganizationEntityId)
            .OnDelete(DeleteBehavior.Cascade);

        // Foreign key to Employee
        builder
            .HasOne(x => x.Employee)
            .WithMany(x => x.OrganizationEntityEmployees)
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Unique constraint (optional — ensure no duplicate org-employee pair)
        builder.HasIndex(x => new { x.OrganizationEntityId, x.EmployeeId }).IsUnique();

        // Index for optimization

        // Additional configuration
    }
}
