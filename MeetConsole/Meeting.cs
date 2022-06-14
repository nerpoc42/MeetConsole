using System.Text.Json.Serialization;

namespace MeetConsole;

internal class Meeting
{
    public string Name { get; }
    public string Description { get; }
    public string ResponsiblePerson { get; }
    public MeetingCategory MeetingCategory { get; }
    public MeetingType MeetingType { get; }
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }
    public Dictionary<string, DateTime> Attendees { get; }

    public Meeting(string name, string description, string responsiblePerson, MeetingCategory meetingCategory,
        MeetingType meetingType,
        DateTime startDate, DateTime endDate)
    {
        Name = name;
        Description = description;
        ResponsiblePerson = responsiblePerson;
        MeetingCategory = meetingCategory;
        MeetingType = meetingType;
        StartDate = startDate;
        EndDate = endDate;
        Attendees = new Dictionary<string, DateTime>();
    }

    [JsonConstructor]
    public Meeting(string name, string description, string responsiblePerson, MeetingCategory category,
        MeetingType type,
        DateTime startDate, DateTime endDate, Dictionary<string, DateTime> attendees)
    {
        Name = name;
        Description = description;
        ResponsiblePerson = responsiblePerson;
        MeetingCategory = category;
        MeetingType = type;
        StartDate = startDate;
        EndDate = endDate;
        Attendees = attendees;
    }

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

internal enum MeetingCategory
{
    CodeMonkey,
    Hub,
    Short,
    TeamBuilding
}

internal enum MeetingType
{
    Live,
    InPerson
}