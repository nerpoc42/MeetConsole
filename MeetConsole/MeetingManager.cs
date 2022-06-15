namespace MeetConsole;

public class MeetingManager
{
    public Dictionary<string, Meeting> Meetings { get; init; } = new();

    public bool TryAddMeeting(Meeting meeting)
    {
        return meeting.Name != null && Meetings.TryAdd(meeting.Name.ToLower(), meeting);
    }

    public bool TryRemoveMeeting(string meetingName, string personName)
    {
        meetingName = meetingName.ToLower();
        personName = personName.ToLower();

        if (!Meetings.TryGetValue(meetingName, out var meeting))
        {
            return false;
        }

        return meeting.ResponsiblePerson == personName && Meetings.Remove(meetingName);
    }

    public bool TryAddAttendeeToMeeting(string meetingName, Attendee attendee)
    {
        meetingName = meetingName.ToLower();
        if (attendee.Name == null)
        {
            return false;
        }

        var attendeeName = attendee.Name.ToLower();

        if (!Meetings.TryGetValue(meetingName, out var meeting))
        {
            return false;
        }

        if (attendee.Date < meeting.StartDate || attendee.Date > meeting.EndDate)
        {
            // Scheduled time does not occur in meeting
            return false;
        }

        return meeting.Attendees.TryAdd(attendeeName, attendee.Date);
    }

    public bool TryRemoveAttendeeFromMeeting(string meetingName, string personName)
    {
        meetingName = meetingName.ToLower();
        personName = personName.ToLower();

        if (!Meetings.TryGetValue(meetingName, out var meeting))
        {
            return false;
        }

        return meeting.ResponsiblePerson != personName && meeting.Attendees.Remove(personName);
    }

    public bool HasIntersectingMeetings(string meetingName, Attendee attendee)
    {
        meetingName = meetingName.ToLower();
        if (attendee.Name == null)
        {
            return false;
        }

        var attendeeName = attendee.Name.ToLower();

        if (!Meetings.TryGetValue(meetingName, out var currentMeeting))
        {
            return false;
        }

        foreach (var (name, meeting) in Meetings)
        {
            if (name == meetingName)
            {
                continue;
            }

            if (!meeting.Attendees.TryGetValue(attendeeName, out var date))
            {
                continue;
            }

            if (attendee.Date > meeting.StartDate && attendee.Date < meeting.EndDate && date < currentMeeting.EndDate)
            {
                return true;
            }
        }

        return false;
    }
}