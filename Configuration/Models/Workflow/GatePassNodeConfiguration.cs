using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Configuration;

namespace portal.Models;

public class GatePassNodeConfiguration : BaseWorkflowNodeConfiguration<GatePassNode>
{
    public override void Configure(EntityTypeBuilder<GatePassNode> builder)
    {
        base.Configure(builder);
        builder.Property(n => n.StepType)
            .IsRequired()
            .HasConversion<string>();

        // Common navigation: workflow
        builder.HasOne(n => n.Workflow)
            .WithMany(w => w.GatePassNodes)
            .HasForeignKey(n => n.WorkflowId)
            .OnDelete(DeleteBehavior.Cascade);        
    }
}