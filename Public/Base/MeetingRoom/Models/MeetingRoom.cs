using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;

namespace portal.Models;

public class MeetingRoom : BaseModel
{
    public string Name { get; set; } = null!;
    public string Location { get; set; } = null!;
    public int Capacity { get; set; }
    public bool IsAvailable { get; set; }
}

public class BookMeetingRoom : BaseModel
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

    public Employee RegisteredBy { get; set; } = null!;
    public Employee User { get; set; } = null!;

    // Minute taker
    public Employee? MinuteTaker { get; set; } = null!;
    public bool IsHasMinuteTaker { get; set; } = false;
    public List<Document> Documents { get; set; } = new List<Document>();

    // Cancellation
    public string? CancellationReason { get; set; }

    // Recurrence
    public bool IsRecurrence { get; set; } = false;
    public RecurrenceRule? RecurrenceRule { get; set; }
}

[Owned]
public class RecurrenceRule
{
    public RecurrenceType RecurrenceType { get; set; }
    public int Interval { get; set; }
    public int OccurrenceCount { get; set; }
    public List<RecurrenceSchedule> RecurrenceSchedules { get; set; } =
        new List<RecurrenceSchedule>();
}
