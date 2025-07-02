namespace portal.DTOs;

public class SurveyResponseDTO : BaseModelWithOnlyIdDTO
{
    public int SurveyQuestionId { get; set; }
    public int EmployeeId { get; set; }
    public List<string> Responses { get; set; } = new();
}

public class SurveyResponseCreateDTO : BaseModelWithOnlyIdCreateDTO
{
    public int SurveyQuestionId { get; set; }
    public int EmployeeId { get; set; }
    public List<string> Responses { get; set; } = new();
}

public class SurveyResponseUpdateDTO : BaseModelWithOnlyIdUpdateDTO { }
