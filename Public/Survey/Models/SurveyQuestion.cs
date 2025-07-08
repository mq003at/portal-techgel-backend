namespace portal.Models;

public class SurveyQuestion : BaseModelWithOnlyId
{
    public int SurveyId { get; set; }
    public Survey Survey { get; set; } = null!;
    public SurveyQuestionType QuestionType { get; set; }
    public List<SurveyResponse> SurveyResponses { get; set; } = new List<SurveyResponse>();

    // Single text option
    public string? Text { get; set; } = null!;

    // Single & Multiple choice options
    public List<string>? Options { get; set; } = new List<string>();
}
