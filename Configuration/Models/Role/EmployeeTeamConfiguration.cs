using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

public class EmployeeTeamConfiguration : IEntityTypeConfiguration<EmployeeTeam>
{
    public void Configure(EntityTypeBuilder<EmployeeTeam> builder)
    {
        builder.HasKey(et => new { et.EmployeeId, et.TeamId });

        builder
            .HasOne(et => et.Employee)
            .WithMany(e => e.EmployeeTeams)
            .HasForeignKey(et => et.EmployeeId);

        builder.HasOne(et => et.Team).WithMany().HasForeignKey(et => et.TeamId);
    }
}
