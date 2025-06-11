using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

namespace portal.Configuration;
public class LeaveRequestNodeConfiguration : BaseWorkflowNodeConfiguration<LeaveRequestNode>
{
    public override void Configure(EntityTypeBuilder<LeaveRequestNode> builder)
    {
        base.Configure(builder);

        builder.Property(n => n.StepType).IsRequired();

        // Any node-specific property configuration goes here.
    }
}