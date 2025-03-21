using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

public class EmployeeUnitConfiguration : IEntityTypeConfiguration<EmployeeUnit>
{
    public void Configure(EntityTypeBuilder<EmployeeUnit> builder)
    {
        builder.HasKey(eu => new { eu.EmployeeId, eu.UnitId });

        builder
            .HasOne(eu => eu.Employee)
            .WithMany(e => e.EmployeeUnits)
            .HasForeignKey(eu => eu.EmployeeId);

        builder.HasOne(eu => eu.Unit).WithMany().HasForeignKey(eu => eu.UnitId);
    }
}
