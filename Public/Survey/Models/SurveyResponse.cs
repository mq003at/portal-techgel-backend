namespace portal.Models;

public class SurveyResponse : BaseModelWithOnlyId
{
    public long SurveyQuestionId { get; set; }
    public SurveyQuestion SurveyQuestion { get; set; } = null!;
    public int EmployeeId { get; set; }
    public List<string> Responses { get; set; } = new List<string>();
}
