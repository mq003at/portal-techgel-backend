using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

namespace portal.Configuration;

public class WorkflowNodeParticipantConfiguration : IEntityTypeConfiguration<WorkflowNodeParticipant>
{
    public void Configure(EntityTypeBuilder<WorkflowNodeParticipant> builder)
    {
        builder.HasKey(e => e.Id); // If using Id as PK
        builder.Property(p => p.EmployeeId);
        builder.Property(p => p.TAT).HasDefaultValue(TimeSpan.Zero);
        builder.Property(p => p.ApprovalStatus).HasConversion<string>();
    }
}