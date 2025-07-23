namespace portal.Models;

public enum MeetingRoomCategory
{
    ALL,
    ADHOC,
    RECURRING
}

public enum BookMeetingRoomStatus
{
    REGISTERING,
    REGISTERED,
    RETURNED,
    USED
}

public enum RecurrenceType
{
    DAY,
    WEEK,
}

public enum RecurrenceSchedule
{
    MONDAY,
    TUESDAY,
    WEDNESDAY,
    THURSDAY,
    FRIDAY,
    SATURDAY,
    SUNDAY,
}

public enum TargetUser
{
    PARTNER,
    INVESTOR,
    INTERNAL, // nội bộ
}
