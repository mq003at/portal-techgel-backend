using DocumentFormat.OpenXml.Math;
using portal.Enums;
using portal.Models;
namespace portal.DTOs;

public abstract class WorkflowNodeDTO : BaseModelDTO
{
    public string Name { get; set; } = null!;

    // Navigation fields
    public List<WorkflowNodeParticipantDTO> WorkflowNodeParticipants { get; set; } = new();
    public List<DocumentAssociationDTO> DocumentAssociations { get; set; } = new();
}

public abstract class WorkflowNodeCreateDTO : BaseModelCreateDTO
{
    public string Name { get; set; } = null!;

    // Only Ids for creation
    public List<WorkflowNodeParticipantCreateDTO>? WorkflowNodeParticipant { get; set; } 
    public List<DocumentAssociationCreateDTO>? DocumentAssociations { get; set; } = new();
}

public abstract class WorkflowNodeUpdateDTO : BaseModelUpdateDTO
{
    // Only Ids for update
    public List<WorkflowNodeParticipantUpdateDTO>? WorkflowNodeParticipants { get; set; } = new();
    public List<DocumentAssociationUpdateDTO>? DocumentAssociations { get; set; } = new();
}

public class ApproverDTO
{
    public int approverId { get; set; }
}

public class RejectDTO
{
    public int approverId { get; set; }
    public string rejectReason { get; set; } = null!;
}
