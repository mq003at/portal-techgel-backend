using portal.Enums;
namespace portal.DTOs;

public class BaseWorkflowDTO : BaseModelDTO
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public GeneralWorkflowStatusType Status { get; set; }

    public List<WorkflowParticipantDTO> WorkflowParticipants { get; set; } = new();
    public List<DocumentAssociationDTO> DocumentAssociations { get; set; } = new();
}

public class BaseWorkflowCreateDTO : BaseModelCreateDTO
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public GeneralWorkflowStatusType Status { get; set; }

    public List<int> WorkflowParticipantIds { get; set; } = new();
    public List<int> DocumentAssociationIds { get; set; } = new();
}

public class BaseWorkflowUpdateDTO : BaseModelUpdateDTO
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public GeneralWorkflowStatusType Status { get; set; }

    public List<int> WorkflowParticipantIds { get; set; } = new();
    public List<int> DocumentAssociationIds { get; set; } = new();
}