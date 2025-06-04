using System.ComponentModel.DataAnnotations;
using portal.Enums;
using portal.Models;
namespace portal.DTOs;

public abstract class BaseWorkflowDTO<T> : BaseDTO<T> where T : BaseWorkflow
{
    [Required]

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public GeneralWorkflowStatusType Status { get; set; }

    public List<int> ReceiverIds { get; set; } = new List<int>();
    public List<string> ReceiverNames { get; set; } = new List<string>();

    public int SenderId { get; set; }
    public string SenderName { get; set; } = string.Empty;

    public List<int> HasBeenApprovedByIds { get; set; } = new List<int>();
    public List<string> HasBeenApprovedByNames { get; set; } = new List<string>();

    public List<DateTime> ApprovedDates { get; set; } = new List<DateTime>();
}

public abstract class CreateBaseWorkflowDTO<T> : BaseDTO<T> where T : BaseWorkflow
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public GeneralWorkflowStatusType Status { get; set; } = GeneralWorkflowStatusType.Draft;

    public List<int> ReceiverIds { get; set; } = new List<int>();
    public required int SenderId { get; set; }
}

public abstract class UpdateBaseWorkflowDTO<T> : BaseDTO<T> where T : BaseWorkflow
{
    public string? Name { get; set; }
    public string? Description { get; set; }

    public GeneralWorkflowStatusType? Status { get; set; }

    public List<int>? ReceiverIds { get; set; }
    public List<int>? HasBeenApprovedByIds { get; set; }
    public List<DateTime>? ApprovedDates { get; set; }
}