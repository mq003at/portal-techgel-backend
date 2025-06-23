using DocumentFormat.OpenXml.Math;
using portal.Enums;
using portal.Models;
namespace portal.DTOs;

public abstract class WorkflowNodeDTO : BaseModelDTO
{
    public string Name { get; set; } = null!;

    // Navigation fields
    public List<WorkflowParticipantDTO> WorkflowNodeParticipants { get; set; } = new();
    public List<DocumentAssociationDTO> DocumentAssociations { get; set; } = new();
}

public abstract class WorkflowNodeCreateDTO : BaseModelCreateDTO
{
    public string Name { get; set; } = null!;

    // Only Ids for creation
    public List<int> WorkflowNodeParticipantIds { get; set; } = new();
    public List<int> DocumentAssociationIds { get; set; } = new();
}

public abstract class WorkflowNodeUpdateDTO : BaseModelUpdateDTO
{
    // Only Ids for update
    public List<int> WorkflowNodeParticipantIds { get; set; } = new();
    public List<int> DocumentAssociationIds { get; set; } = new();
}
