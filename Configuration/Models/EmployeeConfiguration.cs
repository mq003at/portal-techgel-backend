namespace portal.Config.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.MainID).IsRequired(false);

        // Ensure Unique Emails (Both Personal and Company)
        builder.HasIndex(e => e.PersonalEmail).IsUnique();
        builder.HasIndex(e => e.CompanyEmail).IsUnique();

        // Ensure Unique Phone Numbers (Both Personal and Company)
        builder.HasIndex(e => e.PhoneNumber).IsUnique();
        builder.HasIndex(e => e.CompanyNumber).IsUnique();

        // Self-referencing Manager (an Employee manages another Employee)
        builder
            .HasOne(e => e.Manager)
            .WithMany()
            .HasForeignKey(e => e.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relationships: Employee <-> Division (Many-to-Many)
        builder
            .HasMany(e => e.EmployeeDivisions)
            .WithOne(ed => ed.Employee)
            .HasForeignKey(ed => ed.EmployeeId);

        builder
            .HasMany(e => e.EmployeeDepartments)
            .WithOne(ed => ed.Employee)
            .HasForeignKey(ed => ed.EmployeeId);

        builder
            .HasMany(e => e.EmployeeSections)
            .WithOne(es => es.Employee)
            .HasForeignKey(es => es.EmployeeId);

        builder
            .HasMany(e => e.EmployeeUnits)
            .WithOne(eu => eu.Employee)
            .HasForeignKey(eu => eu.EmployeeId);

        builder
            .HasMany(e => e.EmployeeTeams)
            .WithOne(et => et.Employee)
            .HasForeignKey(et => et.EmployeeId);
    }
}
