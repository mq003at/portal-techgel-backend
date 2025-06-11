using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

namespace portal.Configuration;

public class WorkflowNodeParticipantConfiguration : IEntityTypeConfiguration<WorkflowNodeParticipant>
{
    public void Configure(EntityTypeBuilder<WorkflowNodeParticipant> builder)
    {
        builder.HasKey(e => e.Id); // If using Id as PK

        // Example: composite alternate key (enforces uniqueness on both columns)
        builder.HasIndex(e => new { e.WorkflowNodeId, e.EmployeeId }).IsUnique();

        builder.HasOne(e => e.BaseWorkflowNode)
            .WithMany(n => n.WorkflowParticipants)
            .HasForeignKey(e => e.WorkflowNodeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(p => p.WorkflowRole)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(p => p.EmployeeId);
        builder.Property(p => p.TAT).HasDefaultValue(TimeSpan.Zero);

    }
}