using portal.Models;

namespace portal.DTOs;

// Meeting Room
public class MeetingRoomDTO : BaseModelDTO
{
    public string Name { get; set; } = null!;
    public string Location { get; set; } = null!;
    public int Capacity { get; set; }
    public bool IsAvailable { get; set; }
}

public class CreateMeetingRoomDTO : BaseModelCreateDTO
{
    public string Name { get; set; } = null!;
    public string Location { get; set; } = null!;
    public int Capacity { get; set; }
    public bool IsAvailable { get; set; } = true;
}

public class UpdateMeetingRoomDTO : BaseModelUpdateDTO
{
    public string? Name { get; set; } = null!;
    public string? Location { get; set; } = null!;
    public int? Capacity { get; set; }
    public bool? IsAvailable { get; set; } = true;
}

/////////////////////////////////////
/// Book Meeting Room
///

public class BookMeetingRoomDTO : BaseModelDTO
{
    public string Reason { get; set; } = null!;
    public MeetingRoomCategory Category { get; set; }
    public BookMeetingRoomStatus Status { get; set; }
    public RecurrenceType RecurrenceType { get; set; }
    public RecurrenceSchedule RecurrenceSchedule { get; set; }
    public TargetUser TargetUser { get; set; }
    public string? Note { get; set; }

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public int RegisteredById { get; set; }
    public string RegisteredByName { get; set; } = null!;
    public int UserId { get; set; }
    public string UserName { get; set; } = null!;
    public int? MinuteTakerId { get; set; }
    public string? MinuteTakerName { get; set; } = null!;

    public List<DocumentDTO> Documents { get; set; } = new();

    public string? CancellationReason { get; set; }

    public bool IsRecurrence { get; set; }
    public RecurrenceRuleDTO? RecurrenceRule { get; set; }
}

public class BookMeetingRoomCreateDTO : BaseModelCreateDTO
{
    public string Reason { get; set; } = null!;
    public MeetingRoomCategory Category { get; set; }
    public RecurrenceType RecurrenceType { get; set; }
    public RecurrenceSchedule RecurrenceSchedule { get; set; }
    public TargetUser TargetUser { get; set; }
    public string? Note { get; set; }

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public int RegisteredById { get; set; }
    public int UserId { get; set; }
    public int? MinuteTakerId { get; set; }

    public List<DocumentDTO> Documents { get; set; } = new();

    public string? CancellationReason { get; set; }

    public bool IsRecurrence { get; set; } = false;
}

public class BookMeetingRoomUpdateDTO : BaseModelUpdateDTO
{
    public string? Reason { get; set; } = null!;
    public MeetingRoomCategory? Category { get; set; }
    public BookMeetingRoomStatus? Status { get; set; }
    public RecurrenceType? RecurrenceType { get; set; }
    public RecurrenceSchedule? RecurrenceSchedule { get; set; }
    public TargetUser? TargetUser { get; set; }
    public string? Note { get; set; }

    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }

    public int? RegisteredById { get; set; }
    public int? UserId { get; set; }
    public int? MinuteTakerId { get; set; }

    public List<DocumentDTO> Documents { get; set; } = new();

    public string? CancellationReason { get; set; }

    public bool IsRecurrence { get; set; } = false;
}

/// <summary>
/// Recurrence Rule DTO
/// </summary>

public class RecurrenceRuleDTO
{
    public RecurrenceType RecurrenceType { get; set; }
    public int Interval { get; set; }
    public int OccurrenceCount { get; set; }
    public List<RecurrenceSchedule> RecurrenceSchedules { get; set; } = new();
}

public class RecurrenceRuleCreateDTO
{
    public RecurrenceType RecurrenceType { get; set; }
    public int Interval { get; set; }
    public int OccurrenceCount { get; set; }
    public List<RecurrenceSchedule> RecurrenceSchedules { get; set; } = new();
}

public class RecurrenceRuleUpdateDTO
{
    public RecurrenceType? RecurrenceType { get; set; }
    public int? Interval { get; set; }
    public int? OccurrenceCount { get; set; }
    public List<RecurrenceSchedule>? RecurrenceSchedules { get; set; } = new();
}
