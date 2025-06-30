using portal.Enums;

namespace portal.DTOs;

public class BaseWorkflowDTO : BaseModelDTO
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int SenderId { get; set; }
    public string SenderName { get; set; } = null!;
    public GeneralWorkflowStatusType Status { get; set; }

    public List<WorkflowNodeParticipantDTO> WorkflowNodeParticipants { get; set; } = new();
    public List<DocumentDTO> DocumentAssociations { get; set; } = new();
}

// Input the sender information
public class BaseWorkflowCreateDTO : BaseModelCreateDTO
{
    public int SenderId { get; set; }
    public GeneralWorkflowStatusType Status { get; set; } = GeneralWorkflowStatusType.DRAFT;
}

public class BaseWorkflowUpdateDTO : BaseModelUpdateDTO
{
}
