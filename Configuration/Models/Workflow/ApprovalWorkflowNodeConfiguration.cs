using System.Text.Json;
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

        builder.Property(n => n.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(n => n.SenderId).IsRequired();

        builder.Property(n => n.Status)
            .HasConversion<int>() // Store enums as int
            .IsRequired();

        builder.Property(n => n.ApprovalDate);

        builder.Property(n => n.Order);

        // List<int> and List<string> as JSON columns
        builder.Property(n => n.ReceiverIds)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<List<int>>(v, (JsonSerializerOptions)null) ?? new List<int>()
            );
        builder.Property(n => n.ReceiverNames)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null) ?? new List<string>()
            );
        builder.Property(n => n.ReceiverMessages)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null) ?? new List<string>()
            );
        builder.Property(n => n.ApprovalCommentIds)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<List<int>>(v, (JsonSerializerOptions)null) ?? new List<int>()
            );
        builder.Property(n => n.ApprovalComments)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null) ?? new List<string>()
            );
        builder.Property(n => n.DocumentIds)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<List<int>>(v, (JsonSerializerOptions)null) ?? new List<int>()
            );

        // Many-to-one with GeneralWorkflow
        builder.HasOne(n => n.GeneralWorkflow)
            .WithMany(w => w.ApprovalWorkflowNodes)
            .HasForeignKey(n => n.GeneralWorkflowId)
            .OnDelete(DeleteBehavior.Cascade);

        // Many-to-many (or one-to-many) with Documents
        builder.HasMany(n => n.Documents)
            .WithMany() // If you have navigation in Document, set here
            .UsingEntity(j => j.ToTable("ApprovalWorkflowNodeDocuments"));
    }
}
