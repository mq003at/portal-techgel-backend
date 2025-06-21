using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

namespace portal.Configuration.Models;

public class RoleInfoConfiguration
    : BaseModelWithOnlyIdConfiguration<RoleInfo>, // inherit your generic config
        IEntityTypeConfiguration<RoleInfo>
{
    public override void Configure(EntityTypeBuilder<RoleInfo> builder)
    {
        builder.ToTable("RoleInfo");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired()
            .ValueGeneratedNever();

        builder.HasOne(x => x.Employee)
               .WithOne(e => e.RoleInfo)
               .HasForeignKey<RoleInfo>(x => x.EmployeeId)
               .OnDelete(DeleteBehavior.Restrict);

        // One-to-one: RoleInfo ↔ Supervisor (another employee)
        builder.HasOne(x => x.Supervisor)
               .WithMany()
               .HasForeignKey(x => x.SupervisorId)
               .OnDelete(DeleteBehavior.Restrict);

        // One-to-one: RoleInfo ↔ DeputySupervisor (another employee)
        builder.HasOne(x => x.DeputySupervisor)
               .WithMany()
               .HasForeignKey(x => x.DeputySupervisorId)
               .OnDelete(DeleteBehavior.Restrict);

        // Optional GroupId (not enforcing foreign key if Group model doesn't exist)
        builder.Property(x => x.GroupId).IsRequired(false);

        // Ignore SubordinateIds (used only for convenience)
        builder.Ignore(x => x.SubordinateIds);

        // Many-to-many: RoleInfo ↔ Managed Organization Entities
        builder.HasMany(x => x.ManagedOrganizationEntities)
               .WithMany() // Or `.WithMany(e => e.Managers)` if reverse navigation
               .UsingEntity(j => j.ToTable("RoleInfoManagedOrganizationEntities"));

        // One-to-many: RoleInfo ↔ OrganizationEntityEmployee
        builder.HasMany(x => x.OrganizationEntityEmployees)
               .WithOne()
               .HasForeignKey("RoleInfoId")
               .OnDelete(DeleteBehavior.Cascade);
        }
}