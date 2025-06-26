
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Configuration;

namespace portal.Models;

public class GatePassWorkflowConfiguration : BaseWorkflowConfiguration<GatePassWorkflow>
{
    public override void Configure(EntityTypeBuilder<GatePassWorkflow> builder)
    {
        base.Configure(builder); // applies shared workflow settings

        builder.ToTable("GatePassWorkflows");

        builder.Property(x => x.Reason)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.GatePassStartTime)
            .IsRequired();

        builder.Property(x => x.GatePassEndTime)
            .IsRequired();

        builder.HasOne(x => x.Employee)
            .WithMany()
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

