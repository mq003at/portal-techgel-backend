using portal.Enums;
using portal.Models;
namespace portal.DTOs;

public abstract class WorkflowNodeDTO : BaseModelDTO
{
    public string Name { get; set; } = string.Empty;

    // Navigation fields
    public List<WorkflowParticipantDTO> Participants { get; set; } = new();
    public List<DocumentAssociationDTO> DocumentAssociations { get; set; } = new();
}

public abstract class WorkflowNodeCreateDTO : BaseModelCreateDTO
{
    public string Name { get; set; } = string.Empty;

    // Only Ids for creation
    public List<int> ParticipantIds { get; set; } = new();
    public List<int> DocumentAssociationIds { get; set; } = new();
}

public abstract class WorkflowNodeUpdateDTO : BaseModelUpdateDTO
{
    public string Name { get; set; } = string.Empty;

    // Only Ids for update
    public List<int> ParticipantIds { get; set; } = new();
    public List<int> DocumentAssociationIds { get; set; } = new();
}
