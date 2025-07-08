namespace portal.DTOs;

public class SurveyQuestionDTO : BaseModelWithOnlyIdDTO
{
    public int SurveyId { get; set; }
    public SurveyQuestionType QuestionType { get; set; }
    public string? Text { get; set; }
    public List<string> Options { get; set; } = new();
    public List<SurveyResponseDTO> SurveyResponses { get; set; } = new();
}

public class SurveyQuestionCreateDTO : BaseModelWithOnlyIdCreateDTO
{
    public SurveyQuestionType QuestionType { get; set; }
    public string? Text { get; set; }
    public List<string> Options { get; set; } = new(); // Safe to assume never null
}

public class SurveyQuestionUpdateDTO : BaseModelWithOnlyIdUpdateDTO
{
    public SurveyQuestionType QuestionType { get; set; }
    public string? Text { get; set; }
    public List<string> Options { get; set; } = new();
}
