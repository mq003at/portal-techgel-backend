using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Enums;
using portal.Models;

namespace portal.Configuration;

public class SurveyConfiguration : BaseModelConfiguration<Survey>
{
    public override void Configure(EntityTypeBuilder<Survey> builder)
    {
        base.Configure(builder);

        builder.ToTable("Surveys");

        builder.Property(s => s.Status).IsRequired();

        builder.Property(s => s.Name).IsRequired();

        builder.Property(s => s.Description).IsRequired();

        // Sender (1-to-many)
        builder
            .HasOne(s => s.Sender)
            .WithMany() // no navigation on Employee
            .HasForeignKey(s => s.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        // Recipients (many-to-many)
        builder.HasMany(s => s.Recipients).WithMany(); // simple many-to-many without join table customization

        // Survey → Questions (1-to-many)
        builder
            .HasMany(s => s.Questions)
            .WithOne(q => q.Survey)
            .HasForeignKey(q => q.SurveyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class SurveyQuestionConfiguration : BaseModelWithOnlyIdConfiguration<SurveyQuestion>
{
    public override void Configure(EntityTypeBuilder<SurveyQuestion> builder)
    {
        base.Configure(builder);

        builder.Property(q => q.QuestionType).IsRequired().HasConversion<string>();
        builder.Property(q => q.Text).HasMaxLength(1000); // allow null, optional

        builder
            .Property(q => q.Options)
            .HasConversion(
                v => string.Join(";;", v ?? new List<string>()),
                v => v.Split(";;", StringSplitOptions.None).ToList()
            )
            .Metadata.SetValueComparer(GlobalValueComparers.StringListComparer); // Use global comparer for string lists

        builder
            .HasMany(q => q.SurveyResponses)
            .WithOne(r => r.SurveyQuestion)
            .HasForeignKey(r => r.SurveyQuestionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class SurveyResponseConfiguration : BaseModelWithOnlyIdConfiguration<SurveyResponse>
{
    public override void Configure(EntityTypeBuilder<SurveyResponse> builder)
    {
        base.Configure(builder);

        builder.Property(r => r.EmployeeId).IsRequired();

        builder
            .Property(r => r.Responses)
            .HasConversion(
                v => string.Join(";;", v),
                v => v.Split(";;", StringSplitOptions.None).ToList()
            )
            .Metadata.SetValueComparer(GlobalValueComparers.StringListComparer);

        builder
            .HasOne(r => r.SurveyQuestion)
            .WithMany(q => q.SurveyResponses)
            .HasForeignKey(r => r.SurveyQuestionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
