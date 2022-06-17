using MeetConsole.Models;

namespace MeetConsole.Services;

public static class MeetingFilter
{
    public static List<Meeting> FilterByName(List<Meeting> meetings, string name)
    {
        return meetings.FindAll(m => m.Name!.ToLower().Contains(name));
    }

    public static List<Meeting> FilterByDescription(List<Meeting> meetings, string description)
    {
        return meetings.FindAll(m => m.Description!.ToLower().Contains(description));
    }

    public static List<Meeting> FilterByResponsiblePerson(List<Meeting> meetings, string personName)
    {
        return meetings.FindAll(m => m.ResponsiblePerson!.ToLower().Contains(personName));
    }

    public static List<Meeting> FilterByCategory(List<Meeting> meetings, MeetingCategory category)
    {
        return meetings.FindAll(m => m.MeetingCategory == category);
    }

    public static List<Meeting> FilterByType(List<Meeting> meetings, MeetingType type)
    {
        return meetings.FindAll(m => m.MeetingType == type);
    }

    public static List<Meeting> FilterByDate(List<Meeting> meetings, DateInterval interval)
    {
        if (interval.Start != null && interval.End != null)
        {
            return meetings.FindAll(m => m.StartDate >= interval.Start && interval.End <= m.EndDate);
        }

        if (interval.Start != null)
        {
            return meetings.FindAll(m => m.StartDate >= interval.Start);
        }

        return interval.End != null ? meetings.FindAll(m => m.EndDate <= interval.End) : meetings;
    }

    public static List<Meeting> FilterByAttendeesCount(List<Meeting> meetings, NumberFilter<int> filter)
    {
        return filter.Comparison switch
        {
            NumberComparison.Equals => meetings.FindAll(m => m.Attendees.Count == filter.Number),
            NumberComparison.LessThan => meetings.FindAll(m => m.Attendees.Count < filter.Number),
            NumberComparison.GreaterThan => meetings.FindAll(m => m.Attendees.Count > filter.Number),
            _ => meetings
        };
    }
}