using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

namespace portal.Configuration;

public class ApprovalWorkflowNodeConfiguration
    : BaseModelConfiguration<ApprovalWorkflowNode>,
        IEntityTypeConfiguration<ApprovalWorkflowNode>
{
    public override void Configure(EntityTypeBuilder<ApprovalWorkflowNode> builder)
    {
        // BaseModel defaults (Id, CreatedAt, UpdatedAt)
        base.Configure(builder);

        builder.ToTable("ApprovalWorkflowNodes");

        builder.Property(n => n.Name).IsRequired().HasMaxLength(255);

        builder.Property(n => n.SenderId).IsRequired();
        builder.Property(n => n.SenderName).HasMaxLength(255);
        builder.Property(n => n.SenderMessage).HasMaxLength(1000).HasDefaultValue(string.Empty);

        builder.Property(n => n.ReceiverId).IsRequired();
        builder.Property(n => n.ReceiverName).HasMaxLength(255);
        builder.Property(n => n.ReceiverMessage).HasMaxLength(1000).HasDefaultValue(string.Empty);

        builder.Property(n => n.ApprovalStatus).IsRequired();

        builder.Property(n => n.ApprovalDate).HasColumnType("timestamp without time zone");

        builder.Property(n => n.ApprovalComment).HasMaxLength(1000).HasDefaultValue(string.Empty);

        builder.Property(n => n.Order).HasColumnName("SortOrder");

        // Foreign key mapping handled in GeneralWorkflowConfiguration via HasMany/WithOne
    }
}
