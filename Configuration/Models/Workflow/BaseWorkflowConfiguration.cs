using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

namespace portal.Configuration;



public abstract class BaseWorkflowConfiguration<TWorkflow> : BaseModelConfiguration<TWorkflow>
    where TWorkflow : BaseWorkflow
{
    public override void Configure(EntityTypeBuilder<TWorkflow> builder)
    {
        base.Configure(builder);

        builder.Property(w => w.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(w => w.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(w => w.Status)
            .IsRequired()
            .HasConversion<string>();

        // Add any other shared workflow configs here.
    }
}
