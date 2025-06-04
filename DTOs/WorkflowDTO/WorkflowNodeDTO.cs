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

    public List<int> ApprovedByIds { get; set; } = new List<int>();
    public List<string> ApprovedByNames { get; set; } = new List<string>();

    public List<int> HasBeenApprovedByIds { get; set; } = new List<int>();
    public List<string> HasBeenApprovedByNames { get; set; } = new List<string>();

    public List<DateTime> ApprovedDates { get; set; } = new List<DateTime>();

    public List<int> DocumentIds { get; set; } = new List<int>();
    public List<string> DocumentNames { get; set; } = new List<string>();
    public List<string> DocumentUrls { get; set; } = new List<string>();
}

public class CreateWorkflowNodeDTO<T> : BaseDTO<T> where T : WorkflowNode
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public GeneralWorkflowStatusType Status { get; set; } = GeneralWorkflowStatusType.Draft;

    public int SenderId { get; set; }
    public List<int> ApprovedByIds { get; set; } = new List<int>();
    public List<int> HasBeenApprovedByIds { get; set; } = new List<int>();
    public List<DateTime> ApprovedDates { get; set; } = new List<DateTime>();

    public List<int> DocumentIds { get; set; } = new List<int>();
}

public class UpdateWorkflowNodeDTO<T> : BaseDTO<T> where T : WorkflowNode
{
    public string? Name { get; set; }
    public string? Description { get; set; }

    public GeneralWorkflowStatusType? Status { get; set; }

    public int? SenderId { get; set; }

    public List<int>? ApprovedByIds { get; set; }
    public List<int>? HasBeenApprovedByIds { get; set; }
    public List<DateTime>? ApprovedDates { get; set; }

    public List<int>? DocumentIds { get; set; }
}
public class WorkflowNodeWithDocumentsDTO : WorkflowNodeDTO<WorkflowNode>
{
    public List<DocumentDTO> Documents { get; set; } = new List<DocumentDTO>();
}