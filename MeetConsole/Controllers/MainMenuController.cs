using MeetConsole.Interfaces;
using MeetConsole.Models;
using MeetConsole.Services;
using MeetConsole.UI;

namespace MeetConsole.Controllers;

public class MainMenuController : IController
{
    private readonly MeetingManager _manager;
    private readonly string _userName;

    private bool AddToMeeting()
    {
        var meetingName = MeetingUi.ReadString("Meeting name: ");
        var personName = MeetingUi.ReadString("Person name: ");
        var attendanceDate = MeetingUi.ReadDate("Attendance date: ");
        var attendee = new Attendee
        {
            Name = personName,
            Date = attendanceDate
        };
        
        if (_manager.TryAddAttendeeToMeeting(meetingName, attendee))
        {
            if (_manager.HasIntersectingMeetings(meetingName, attendee))
            {
                Console.WriteLine("Warning: Attendee's meetings intersect");
            }
            
            return true;
        }
        
        Console.WriteLine("Failed to add attendee to a meeting");
        return false;

    }

    private bool CreateMeeting()
    {
        var name = MeetingUi.ReadString("Meeting name: ");
        var description = MeetingUi.ReadString("Meeting description: ");
        var category = MeetingUi.ReadEnum<MeetingCategory>(
            "Meeting Category (Valid: CodeMonkey | Hub | Short | TeamBuilding): ");
        var type = MeetingUi.ReadEnum<MeetingType>("Meeting Type (Valid: Live | InPerson): ");
        var startDate = MeetingUi.ReadDate("Meeting Start Date: ");
        var endDate = MeetingUi.ReadDate("Meeting End Date: ");

        var meeting = new Meeting
        {
            Name = name,
            Description = description,
            ResponsiblePerson = _userName,
            MeetingCategory = category,
            MeetingType = type,
            StartDate = startDate,
            EndDate = endDate
        };
        
        if (_manager.TryAddMeeting(meeting))
        {
            return true;
        }
        
        Console.WriteLine("Failed to create a meeting");
        return false;
    }

    private bool RemoveFromMeeting()
    {
        var meetingName = MeetingUi.ReadString("Meeting name: ");
        var personName = MeetingUi.ReadString("Person name: ");
        
        if (_manager.TryRemoveAttendeeFromMeeting(meetingName, personName))
        {
            return true;
        }
        
        Console.WriteLine("Failed to remove attendee from a meeting");
        return false;
    }

    private bool RemoveMeeting()
    {
        var meetingName = MeetingUi.ReadString("Meeting name: ");
        
        if (_manager.TryRemoveMeeting(meetingName, _userName))
        {
            return true;
        }
        
        Console.WriteLine("Failed to remove a meeting");
        return false;
    }

    public MainMenuController(MeetingManager manager)
    {
        _manager = manager;
        _userName = MeetingUi.ReadString("Enter your name: ");
    }

    public void Control()
    {
        MeetingUi.DisplayMainMenu();
        var success = false;

        while (true)
        {
            var choice = (MainMenuChoice)MeetingUi.ReadNumber();
            switch (choice)
            {
                case MainMenuChoice.AddPerson:
                    success = AddToMeeting();
                    break;
                case MainMenuChoice.CreateMeeting:
                    success = CreateMeeting();
                    break;
                case MainMenuChoice.RemoveMeeting:
                    success = RemoveMeeting();
                    break;
                case MainMenuChoice.RemovePerson:
                    success = RemoveFromMeeting();
                    break;
                case MainMenuChoice.ListMeetings:
                    new FilterMenuController(_manager.GetMeetings()).Control();
                    return;
                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }

            if (success)
            {
                break;
            }
            
            Console.WriteLine("Try again: ");
        }
    }
}