namespace MeetConsole.Models;

public class Meeting
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? ResponsiblePerson { get; init; }
    public MeetingCategory MeetingCategory { get; init; }
    public MeetingType MeetingType { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public Dictionary<string, DateTime> Attendees { get; } = new();

    public override string ToString()
    {
        return $"Name: {Name}\n" +
               $"Description: {Description}\n" +
               $"Responsible user: {ResponsiblePerson}\n" +
               $"Category: {MeetingCategory} \n" +
               $"Type: {MeetingType} \n" +
               $"Starting date: {StartDate:yyyy-MM-dd} \n" +
               $"Ending date: {EndDate:yyyy-MM-dd} \n" +
               $"Number of attendees: {Attendees.Count} \n";
    }
}