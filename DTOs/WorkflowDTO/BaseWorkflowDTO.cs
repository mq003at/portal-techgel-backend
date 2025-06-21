using portal.Enums;
namespace portal.DTOs;

public class BaseWorkflowDTO : BaseModelDTO
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public GeneralWorkflowStatusType Status { get; set; }

    public List<WorkflowParticipantDTO> WorkflowParticipants { get; set; } = new();
    public List<DocumentAssociationDTO> DocumentAssociations { get; set; } = new();
}

// Input the sender information
public class BaseWorkflowCreateDTO : BaseModelCreateDTO
{
    public int SenderId { get; set; }
}

public class BaseWorkflowUpdateDTO : BaseModelUpdateDTO
{
    public GeneralWorkflowStatusType? Status { get; set; }
}