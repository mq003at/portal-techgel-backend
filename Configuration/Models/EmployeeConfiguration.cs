using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

namespace portal.Configuration.Models;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(e => e.Id);

        // Owned value objects
        builder.OwnsOne(e => e.PersonalInfo);
        builder.OwnsOne(e => e.CompanyInfo);
        builder.OwnsOne(e => e.CareerPathInfo);
        builder.OwnsOne(e => e.TaxInfo);
        builder.OwnsOne(e => e.InsuranceInfo);
        builder.OwnsOne(e => e.EmergencyContactInfo);
        builder.OwnsOne(e => e.ScheduleInfo);
    }
}
