using System.Text.Json;

namespace MeetConsole;

public static class Program
{
    public static void Main()
    {
        const string fileName = "meetings.json";

        MeetingManager manager;
        try
        {
            var text = File.ReadAllText(fileName);
            manager = JsonSerializer.Deserialize<MeetingManager>(text) ?? new MeetingManager();
        }
        catch (Exception)
        {
            manager = new MeetingManager();
        }

        MeetingInterface.ServeMainMenu(ref manager);

        File.WriteAllText(fileName, JsonSerializer.Serialize(manager));
    }
}