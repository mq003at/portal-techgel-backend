using portal.Enums;

namespace portal.DTOs;

public class BaseWorkflowDTO : BaseModelDTO
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int SenderId { get; set; }
    public string SenderMainId { get; set; } = null!;
    public string SenderName { get; set; } = null!;
    public GeneralWorkflowStatusType Status { get; set; }

    public List<WorkflowNodeParticipantDTO> WorkflowNodeParticipants { get; set; } = new();
    public List<DocumentDTO> DocumentAssociations { get; set; } = new();
    public bool IsDocumentGenerated { get; set; }
}

// Input the sender information
public class BaseWorkflowCreateDTO : BaseModelCreateDTO
{
    public int SenderId { get; set; }
}

public class BaseWorkflowUpdateDTO : BaseModelUpdateDTO { }
