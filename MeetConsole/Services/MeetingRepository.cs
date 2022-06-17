using System.Text.Json;
using MeetConsole.Models;

namespace MeetConsole.Services;

public class MeetingRepository 
{
    private const string FileName = "meetings.json";
    private readonly string _fileName;
    private readonly Dictionary<string, Meeting> _meetings;

    public Dictionary<string, Meeting> GetMeetings()
    {
        return _meetings;
    }

    public void SaveMeetings()
    {
        File.WriteAllText(_fileName, JsonSerializer.Serialize(_meetings));
    }

    public MeetingRepository(string fileName = FileName)
    {
        _fileName = fileName;
        try
        {
            var text = File.ReadAllText(_fileName);
            _meetings = JsonSerializer.Deserialize<Dictionary<string, Meeting>>(text) ??
                       new Dictionary<string, Meeting>();
        }
        catch (Exception)
        {
            _meetings = new Dictionary<string, Meeting>();
        }
    }
}