namespace portal.DTOs;

public class SurveyDTO : BaseModelDTO
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<SurveyQuestionDTO> Questions { get; set; } = new();
    public string SenderName { get; set; } = null!;
    public List<string> RecipientNames { get; set; } = new();
}

public class SurveyCreateDTO : BaseModelCreateDTO
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<SurveyQuestionCreateDTO> Questions { get; set; } = new();
    public int SenderId { get; set; }
    public List<int> RecipientIds { get; set; } = new();
}

public class SurveyUpdateDTO : BaseModelUpdateDTO
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<SurveyQuestionUpdateDTO> Questions { get; set; } = new();
    public int SenderId { get; set; }
    public List<int> RecipientIds { get; set; } = new();
}
