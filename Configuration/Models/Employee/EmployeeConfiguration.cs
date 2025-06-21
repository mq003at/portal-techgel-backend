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

        // 4) Relationships (replace owned types with entity relationships)

        // 4.1) One-to-one relationships
        builder
            .HasOne(e => e.PersonalInfo)
            .WithOne(p => p.Employee)
            .HasForeignKey<PersonalInfo>(p => p.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(e => e.CompanyInfo)
            .WithOne(c => c.Employee)
            .HasForeignKey<CompanyInfo>(c => c.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(e => e.CareerPathInfo)
            .WithOne(c => c.Employee)
            .HasForeignKey<CareerPathInfo>(c => c.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(e => e.TaxInfo)
            .WithOne(t => t.Employee)
            .HasForeignKey<TaxInfo>(t => t.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(e => e.InsuranceInfo)
            .WithOne(i => i.Employee)
            .HasForeignKey<InsuranceInfo>(i => i.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(e => e.EmergencyContactInfos)
            .WithOne(ec => ec.Employee)
            .HasForeignKey(ec => ec.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(e => e.EmployeeQualificationInfos)
            .WithOne(eq => eq.Employee)
            .HasForeignKey(eq => eq.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        // If ScheduleInfo is a single object (1-1), use this:
        builder
            .HasOne(e => e.ScheduleInfo)
            .WithOne(s => s.Employee)
            .HasForeignKey<ScheduleInfo>(s => s.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        // 5) Signature relationship
        builder
            .HasOne(e => e.Signature)
            .WithOne(s => s.Employee)
            .HasForeignKey<Signature>(s => s.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        // 6) RoleInfo (owned one-to-one)
        builder
            .HasOne(e => e.RoleInfo)
            .WithOne(r => r.Employee)
            .HasForeignKey<RoleInfo>(r => r.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade
    );
    }
}
