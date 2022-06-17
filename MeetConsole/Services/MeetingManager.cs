using MeetConsole.Models;

namespace MeetConsole.Services;

public class MeetingManager
{
    private readonly MeetingRepository _repo;

    public MeetingManager(MeetingRepository repo)
    {
        _repo = repo;
    }

    public List<Meeting> GetMeetings()
    {
        return _repo.GetMeetings().Values.ToList();
    }

    public bool TryAddMeeting(Meeting meeting)
    {
        if (meeting.Name == null || !_repo.GetMeetings().TryAdd(meeting.Name.ToLower(), meeting))
        {
            return false;
        }

        _repo.SaveMeetings();
        return true;
    }

    public bool TryRemoveMeeting(string meetingName, string personName)
    {
        meetingName = meetingName.ToLower();
        personName = personName.ToLower();

        if (!_repo.GetMeetings().TryGetValue(meetingName, out var meeting))
        {
            return false;
        }

        if (meeting.ResponsiblePerson != personName || !_repo.GetMeetings().Remove(meetingName))
        {
            return false;
        }
        
        _repo.SaveMeetings();
        return true;
    }

    public bool TryAddAttendeeToMeeting(string meetingName, Attendee attendee)
    {
        meetingName = meetingName.ToLower();
        if (attendee.Name == null)
        {
            return false;
        }

        var attendeeName = attendee.Name.ToLower();

        if (!_repo.GetMeetings().TryGetValue(meetingName, out var meeting))
        {
            return false;
        }

        if (attendee.Date < meeting.StartDate || attendee.Date > meeting.EndDate)
        {
            // Scheduled time does not occur in meeting
            return false;
        }

        if (!meeting.Attendees.TryAdd(attendeeName, attendee.Date))
        {
            return false;
        }
        
        _repo.SaveMeetings();
        return true;

    }

    public bool TryRemoveAttendeeFromMeeting(string meetingName, string personName)
    {
        meetingName = meetingName.ToLower();
        personName = personName.ToLower();

        if (!_repo.GetMeetings().TryGetValue(meetingName, out var meeting))
        {
            return false;
        }

        if (meeting.ResponsiblePerson == personName || !meeting.Attendees.Remove(personName))
        {
            return false;
        }
        
        _repo.SaveMeetings();
        return true;

    }

    public bool HasIntersectingMeetings(string meetingName, Attendee attendee)
    {
        meetingName = meetingName.ToLower();
        if (attendee.Name == null)
        {
            return false;
        }

        var attendeeName = attendee.Name.ToLower();

        if (!_repo.GetMeetings().TryGetValue(meetingName, out var currentMeeting))
        {
            return false;
        }

        foreach (var (name, meeting) in _repo.GetMeetings())
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