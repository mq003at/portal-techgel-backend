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

    public ICollection<int> ReceiverIds { get; set; } = new List<int>();
    public ICollection<string> ReceiverNames { get; set; } = new List<string>();

    public int SenderId { get; set; }
    public string SenderName { get; set; } = string.Empty;

    public ICollection<int> HasBeenApprovedByIds { get; set; } = new List<int>();
    public ICollection<string> HasBeenApprovedByNames { get; set; } = new List<string>();

    public ICollection<DateTime> ApprovedDates { get; set; } = new List<DateTime>();
}

public abstract class CreateBaseWorkflowDTO<T> : BaseDTO<T> where T : BaseWorkflow
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public GeneralWorkflowStatusType Status { get; set; } = GeneralWorkflowStatusType.Draft;

    public ICollection<int> ReceiverIds { get; set; } = new List<int>();
    public int SenderId { get; set; }
}

public abstract class UpdateBaseWorkflowDTO<T> : BaseDTO<T> where T : BaseWorkflow
{
    public string? Name { get; set; }
    public string? Description { get; set; }

    public GeneralWorkflowStatusType? Status { get; set; }

    public ICollection<int>? ReceiverIds { get; set; }
    public ICollection<int>? HasBeenApprovedByIds { get; set; }
    public ICollection<DateTime>? ApprovedDates { get; set; }
}