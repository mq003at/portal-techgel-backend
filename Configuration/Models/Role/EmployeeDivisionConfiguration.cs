using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

public class EmployeeDivisionConfiguration : IEntityTypeConfiguration<EmployeeDivision>
{
    public void Configure(EntityTypeBuilder<EmployeeDivision> builder)
    {
        builder.HasKey(ed => new { ed.EmployeeId, ed.DivisionId });

        builder
            .HasOne(ed => ed.Employee)
            .WithMany(e => e.EmployeeDivisions)
            .HasForeignKey(ed => ed.EmployeeId);

        builder.HasOne(ed => ed.Division).WithMany().HasForeignKey(ed => ed.DivisionId);
    }
}
