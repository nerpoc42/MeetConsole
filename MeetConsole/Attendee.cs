namespace MeetConsole;

internal class Attendee
{
    public string Name { get; }
    public DateTime Date { get; }

    public Attendee(string name, DateTime date)
    {
        Name = name;
        Date = date;
    }
}