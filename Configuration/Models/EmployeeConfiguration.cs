using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

namespace portal.Configuration.Models;

public class EmployeeConfiguration
    : BaseModelConfiguration<Employee>, // inherit your generic config
        IEntityTypeConfiguration<Employee>
{
    public override void Configure(EntityTypeBuilder<Employee> builder)
    {
        // 1) Apply the BaseModelConfiguration<Employee> so CreatedAt/UpdatedAt defaults are set
        base.Configure(builder);

        // 2) Table & key (optional—BaseModelConfiguration didn’t set ToTable)
        builder.ToTable("Employees");
        builder.HasKey(e => e.Id);

        // 3) Scalar props
        builder.Property(e => e.MainId).IsRequired(true);

        builder.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(e => e.MiddleName).HasMaxLength(100);
        builder.Property(e => e.LastName).IsRequired().HasMaxLength(100);

        builder.Property(e => e.Avatar).HasMaxLength(255);
        builder.Property(e => e.Password).HasMaxLength(255);

        // 4) Owned types
        builder.OwnsOne(e => e.PersonalInfo);
        builder.OwnsOne(e => e.CompanyInfo);
        builder.OwnsOne(e => e.CareerPathInfo);
        builder.OwnsOne(e => e.TaxInfo);
        builder.OwnsOne(e => e.InsuranceInfo);
        builder.OwnsOne(e => e.EmergencyContactInfo);
        builder.OwnsOne(e => e.ScheduleInfo);
        builder.OwnsOne(e => e.RoleInfo);

        // 5) Signature relationship
        builder
            .HasOne(e => e.Signature)
            .WithOne(s => s.Employee)
            .HasForeignKey<Signature>(s => s.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);



        // 6) RoleInfo (owned one-to-one)
        builder.OwnsOne(
    e => e.RoleInfo,
    ri =>
    {
        // map Supervisor & Group
        ri.Property(r => r.SupervisorId).IsRequired(false).HasColumnName("SupervisorId");
        ri.Property(r => r.GroupId).IsRequired(false).HasColumnName("GroupId");

        // Không cần HasConversion!
        ri.Property(r => r.ManagedOrganizationEntityIds)
            .HasColumnName("ManagedOrganizationEntityIds");

        ri.Property(r => r.SubordinateIds)
            .HasColumnName("SubordinateIds");

        // supervisor FK
        ri.HasOne(r => r.Supervisor)
            .WithMany()
            .HasForeignKey(r => r.SupervisorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
);
    }
}
