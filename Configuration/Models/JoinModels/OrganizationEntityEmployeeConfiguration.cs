using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace portal.Configuration;

public class OrganizationEntityEmployeeConfiguration : IEntityTypeConfiguration<OrganizationEntityEmployee>
{
    public void Configure(EntityTypeBuilder<OrganizationEntityEmployee> builder)
    {
        // Composite Key
        builder.HasKey(x => new { x.OrganizationEntityId, x.EmployeeId });

        // Relationships
        builder.HasOne(x => x.OrganizationEntity)
               .WithMany(x => x.OrganizationEntityEmployees)
               .HasForeignKey(x => x.OrganizationEntityId)
               .OnDelete(DeleteBehavior.Cascade); 

        builder.HasOne(x => x.Employee)
               .WithMany(x => x.OrganizationEntityEmployees)
               .HasForeignKey(x => x.EmployeeId)
               .OnDelete(DeleteBehavior.Cascade); 

        builder.Property(x => x.OrganizationRelationType)
               .HasConversion<string>()
               .IsRequired();

        builder.HasIndex(x => new { x.OrganizationEntityId, x.IsPrimary });

        builder.Property(x => x.IsPrimary)
               .HasComment("Marks the primary association of this employee to the org entity.");
    }
}