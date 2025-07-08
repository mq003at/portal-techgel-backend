using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

namespace portal.Configuration;

public class WorkflowNodeParticipantConfiguration
    : IEntityTypeConfiguration<WorkflowNodeParticipant>
{
    public void Configure(EntityTypeBuilder<WorkflowNodeParticipant> builder)
    {
        builder.HasKey(e => e.Id); // If using Id as PK
        builder.Property(p => p.EmployeeId);
        builder.Property(p => p.TAT).HasDefaultValue(TimeSpan.Zero);
        builder.Property(p => p.ApprovalStatus).HasConversion<string>();
        builder.Property(p => p.WorkflowNodeType).IsRequired();
        builder.Property(p => p.ApprovalStartDate).IsRequired(false);
        builder.Property(p => p.ApprovalDate).IsRequired(false);
        builder.Property(p => p.ApprovalDeadline).IsRequired(false);
        builder.Property(p => p.WorkflowNodeId).IsRequired();
        builder.Property(p => p.WorkflowId).IsRequired();
        builder
            .HasOne(e => e.Employee)
            .WithMany()
            .HasForeignKey(e => e.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
