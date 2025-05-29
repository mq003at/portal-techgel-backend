using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

namespace portal.Configuration;

public class GeneralWorkflowConfiguration
    : BaseModelConfiguration<GeneralWorkflow>,
        IEntityTypeConfiguration<GeneralWorkflow>
{
    public override void Configure(EntityTypeBuilder<GeneralWorkflow> builder)
    {
        base.Configure(builder);

        builder.ToTable("GeneralWorkflows");

        builder.OwnsOne(
            w => w.GeneralWorkflowInfo,
            g =>
            {
                g.Property(x => x.Name).IsRequired().HasMaxLength(255);
                g.Property(x => x.Description).HasMaxLength(1000);
                g.Property(x => x.Status).IsRequired();
                g.Property(x => x.WorkflowLogic).IsRequired();

                g.Property(x => x.Quota);

                g.Ignore(x => x.ApprovedByNames);
                g.Ignore(x => x.DraftedByNames);
            }
        );
    }
}
