using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace portal.Models.Configurations;

public class EmployeeRoleDetailConfiguration : IEntityTypeConfiguration<EmployeeRoleDetail>
{
    public void Configure(EntityTypeBuilder<EmployeeRoleDetail> builder)
    {
        builder.HasKey(rd => new { rd.EmployeeId, rd.OrganizationEntityId });

        builder
            .HasOne(rd => rd.Employee)
            .WithMany(e => e.RoleDetails)
            .HasForeignKey(rd => rd.EmployeeId);

        builder
            .HasOne(rd => rd.OrganizationEntity)
            .WithMany()
            .HasForeignKey(rd => rd.OrganizationEntityId);

        // nếu cần thêm navigation cho ManagesOrganizationEntity, Subordinate…
    }
}
