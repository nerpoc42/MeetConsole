using System.Globalization;

namespace MeetConsole;

public static class AttendeeCollection
{
    // Cannot do reference to meeting.
    public static Dictionary<string, Dictionary<int, DateTime>> AttendeeList { get; set; } = new();

    public static bool AddToMeeting(string personName, int meetingId, DateTime joinDate)
    {
        var meeting = MeetingCollection.GetMeeting(meetingId);

        if (!AttendeeList.ContainsKey(personName))
        {
            AttendeeList.Add(personName,
                new Dictionary<int, DateTime>());
        }

        if (joinDate < meeting.StartDate || joinDate > meeting.EndDate)
        {
            return false;
        }

        foreach (var scheduledJoin in AttendeeList[personName])
        {
            var scheduledMeeting = MeetingCollection.GetMeeting(scheduledJoin.Key);

            if (!(scheduledMeeting.EndDate <= meeting.EndDate) || !(joinDate >= scheduledMeeting.StartDate)) continue;
            Console.WriteLine("Warning: Person is present in multiple meetings.");
            break;
        }

        if (AttendeeList[personName].ContainsKey(meetingId))
        {
            return false;
        }

        AttendeeList[personName].Add(meetingId, joinDate);
        meeting.Attendees.Add(personName, joinDate);
        return true;
    }

    public static bool RemoveFromMeeting(string personName, int meetingId)
    {
        if (!AttendeeList.ContainsKey(personName))
        {
            return false;
        }

        if (MeetingCollection.GetMeeting(meetingId).ResponsiblePerson == personName)
        {
            return false;
        }

        MeetingCollection.GetMeeting(meetingId).Attendees.Remove(personName);
        AttendeeList[personName].Remove(meetingId);

        return true;
    }
}

public enum MeetingCategory
{
    CodeMonkey,
    Hub,
    Short,
    TeamBuilding
}

public enum MeetingType
{
    Live,
    InPerson
}

// Rewrite in class maybe?
public class Meeting
{
    public string? Name { get; set; }
    public string? ResponsiblePerson { get; set; }
    public string? Description { get; set; }
    public MeetingCategory? Category { get; set; }
    public MeetingType? Type { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Dictionary<string, DateTime> Attendees { get; }

    public Meeting()
    {
        Attendees = new Dictionary<string, DateTime>();
    }

    public override string ToString()
    {
        return @$"Meeting Name: {Name ?? "None"}
Description: {Description ?? "None"}
Responsible Person: {ResponsiblePerson ?? "None"}
Category: {(Category != null ? Enum.GetName((MeetingCategory)Category) : "None")}            
Type: {(Category != null ? Enum.GetName((MeetingCategory)Category) : "None")}
Start Date: {StartDate?.ToString(CultureInfo.InvariantCulture) ?? "None"}
End Date: {EndDate?.ToString(CultureInfo.InvariantCulture) ?? "None"}
Attendees Count: {Attendees.Count}";
    }
}

public static class MeetingCollection
{
    private static int _nextId;

    private static Dictionary<int, Meeting> _meetings = new();

    public static Dictionary<int, Meeting> Meetings
    {
        get => _meetings;
        set
        {
            _meetings = value;
            _nextId = _meetings.Last().Key + 1;
        }
    }

    public static Meeting GetMeeting(int meetingId)
    {
        return Meetings[meetingId];
    }

    public static int CreateMeeting(Meeting meeting)
    {
        var id = _nextId;
        Meetings.Add(id, meeting);
        ++_nextId;
        return id;
    }

    public static bool RemoveMeeting(int meetingId, string personName)
    {
        if (!Meetings.ContainsKey(meetingId))
        {
            return false;
        }
        
        var meeting = Meetings[meetingId];
        if (meeting.ResponsiblePerson != personName)
        {
            return false;
        }

        foreach (var attendee in meeting.Attendees)
        {
            if (!AttendeeCollection.RemoveFromMeeting(attendee.Key, meetingId))
            {
                return false;
            }
        }
        
        return Meetings.Remove(meetingId);
    }
}

public struct MeetingData
{
    public Dictionary<string, Dictionary<int, DateTime>> Attendees { get; set; }
    public Dictionary<int, Meeting> Meetings { get; set; }
}