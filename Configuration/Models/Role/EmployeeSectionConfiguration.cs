using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

public class EmployeeSectionConfiguration : IEntityTypeConfiguration<EmployeeSection>
{
    public void Configure(EntityTypeBuilder<EmployeeSection> builder)
    {
        builder.HasKey(es => new { es.EmployeeId, es.SectionId });

        builder
            .HasOne(es => es.Employee)
            .WithMany(e => e.EmployeeSections)
            .HasForeignKey(es => es.EmployeeId);

        builder.HasOne(es => es.Section).WithMany().HasForeignKey(es => es.SectionId);
    }
}
