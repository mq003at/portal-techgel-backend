using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

public class EmployeeDepartmentConfiguration : IEntityTypeConfiguration<EmployeeDepartment>
{
    public void Configure(EntityTypeBuilder<EmployeeDepartment> builder)
    {
        builder.HasKey(ed => new { ed.EmployeeId, ed.DepartmentId });

        builder
            .HasOne(ed => ed.Employee)
            .WithMany(e => e.EmployeeDepartments)
            .HasForeignKey(ed => ed.EmployeeId);

        builder.HasOne(ed => ed.Department).WithMany().HasForeignKey(ed => ed.DepartmentId);
    }
}
