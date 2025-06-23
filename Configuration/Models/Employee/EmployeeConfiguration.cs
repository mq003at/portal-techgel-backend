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
        base.Configure(builder);

        builder.ToTable("Employees");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.MainId).IsRequired(true);

        builder.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(e => e.MiddleName).HasMaxLength(100);
        builder.Property(e => e.LastName).IsRequired().HasMaxLength(100);

        builder.Property(e => e.Avatar).HasMaxLength(255);
        builder.Property(e => e.Password).HasMaxLength(255);


        // One-to-one relationships
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

        builder
            .HasOne(e => e.Signature)
            .WithOne(s => s.Employee)
            .HasForeignKey<Signature>(s => s.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Roleinfo
        builder
            .HasMany(e => e.OrganizationEntityEmployees)
            .WithOne(oe => oe.Employee)
            .HasForeignKey(oe => oe.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Supervisor)
            .WithMany(e => e.Subordinates)
            .HasForeignKey(e => e.SupervisorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.DeputySupervisor)
            .WithMany(e => e.DeputySubordinates)
            .HasForeignKey(e => e.DeputySupervisorId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
