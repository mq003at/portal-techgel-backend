namespace portal.Models;

public class Survey : BaseModel
{
    public SurveyStatus Status { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public int SenderId { get; set; }
    public Employee Sender { get; set; } = null!;
    public int RecipientIds { get; set; }
    public List<Employee> Recipients { get; set; } = new List<Employee>();

    public List<SurveyQuestion> Questions { get; set; } = new List<SurveyQuestion>();
}
