// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using MeetConsole;

const string fileName = "save.json";
try
{
    var text = File.ReadAllText(fileName);

    var data = JsonSerializer.Deserialize<MeetingData>(text);

    AttendeeCollection.AttendeeList = data.Attendees;
    MeetingCollection.Meetings = data.Meetings;
}
catch (FileNotFoundException)
{
    Console.WriteLine("Could not load meetings from a file");
} 

new MeetingMainMenu().Serve();

var jsonString = JsonSerializer.Serialize(new MeetingData
    { Meetings = MeetingCollection.Meetings, Attendees = AttendeeCollection.AttendeeList });

File.WriteAllText(fileName, jsonString);