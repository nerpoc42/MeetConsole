using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace MeetConsole;

internal class MeetingManager
{
    private readonly Dictionary<string, Meeting> _meetings;

    public ReadOnlyDictionary<string, Meeting> Meetings => new(_meetings);

    public MeetingManager()
    {
        _meetings = new Dictionary<string, Meeting>();
    }

    [JsonConstructor]
    public MeetingManager(Dictionary<string, Meeting> meetings)
    {
        _meetings = new Dictionary<string, Meeting>(meetings);
    }

    public bool TryAddMeeting(Meeting meeting)
    {
        return _meetings.TryAdd(meeting.Name.ToLower(), meeting);
    }

    public bool TryRemoveMeeting(string meetingName, string personName)
    {
        meetingName = meetingName.ToLower();
        personName = personName.ToLower();
        
        if (!_meetings.TryGetValue(meetingName, out var meeting))
        {
            return false;
        }

        return meeting.ResponsiblePerson == personName && _meetings.Remove(meetingName);
    }

    public bool TryAddAttendeeToMeeting(string meetingName, Attendee attendee)
    {
        meetingName = meetingName.ToLower();
        var attendeeName = attendee.Name.ToLower();
        
        if (!_meetings.TryGetValue(meetingName, out var meeting))
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
        
        if (!_meetings.TryGetValue(meetingName, out var meeting))
        {
            return false;
        }

        return meeting.ResponsiblePerson != personName && meeting.Attendees.Remove(personName);
    }

    public bool HasIntersectingMeetings(string meetingName, Attendee attendee)
    {
        meetingName = meetingName.ToLower();
        var attendeeName = attendee.Name.ToLower();
        
        if (!_meetings.TryGetValue(meetingName, out var currentMeeting))
        {
            return false;
        }

        foreach (var (name, meeting) in _meetings)
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