using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

namespace portal.Configuration;

public abstract class BaseWorkflowNodeConfiguration<TNode> : BaseModelConfiguration<TNode>
    where TNode : BaseWorkflowNode
{
    public override void Configure(EntityTypeBuilder<TNode> builder)
    {
        base.Configure(builder);

        builder.Property(n => n.Name).IsRequired().HasMaxLength(255);

        builder.Property(n => n.Description).HasMaxLength(1000);

        builder.Property(n => n.Status).IsRequired().HasConversion<string>();
    }
}
