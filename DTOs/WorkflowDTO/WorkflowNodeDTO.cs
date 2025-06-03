using portal.Enums;
using portal.Models;
namespace portal.DTOs;

public class WorkflowNodeDTO<T> : BaseDTO<T> where T : WorkflowNode
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public GeneralWorkflowStatusType Status { get; set; }

    public int SenderId { get; set; }
    public string SenderName { get; set; } = string.Empty;

    public ICollection<int> ApprovedByIds { get; set; } = new List<int>();
    public ICollection<string> ApprovedByNames { get; set; } = new List<string>();

    public ICollection<int> HasBeenApprovedByIds { get; set; } = new List<int>();
    public ICollection<string> HasBeenApprovedByNames { get; set; } = new List<string>();

    public ICollection<DateTime> ApprovedDates { get; set; } = new List<DateTime>();

    public ICollection<int> DocumentIds { get; set; } = new List<int>();
    public ICollection<string> DocumentNames { get; set; } = new List<string>();
    public ICollection<string> DocumentUrls { get; set; } = new List<string>();
}

public class CreateWorkflowNodeDTO<T> : BaseDTO<T> where T : WorkflowNode
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public GeneralWorkflowStatusType Status { get; set; } = GeneralWorkflowStatusType.Draft;

    public int SenderId { get; set; }
    public ICollection<int> ApprovedByIds { get; set; } = new List<int>();
    public ICollection<int> HasBeenApprovedByIds { get; set; } = new List<int>();
    public ICollection<DateTime> ApprovedDates { get; set; } = new List<DateTime>();

    public ICollection<int> DocumentIds { get; set; } = new List<int>();
}

public class UpdateWorkflowNodeDTO<T> : BaseDTO<T> where T : WorkflowNode
{
    public string? Name { get; set; }
    public string? Description { get; set; }

    public GeneralWorkflowStatusType? Status { get; set; }

    public int? SenderId { get; set; }

    public ICollection<int>? ApprovedByIds { get; set; }
    public ICollection<int>? HasBeenApprovedByIds { get; set; }
    public ICollection<DateTime>? ApprovedDates { get; set; }

    public ICollection<int>? DocumentIds { get; set; }
}
public class WorkflowNodeWithDocumentsDTO : WorkflowNodeDTO<WorkflowNode>
{
    public ICollection<DocumentDTO> Documents { get; set; } = new List<DocumentDTO>();
}