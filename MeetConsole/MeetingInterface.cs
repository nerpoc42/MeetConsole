namespace MeetConsole;

public static class MeetingInterface
{
    private static string ReadString(string display)
    {
        string name;
        Console.Write(display);

        while ((name = Console.ReadLine()!).Length == 0)
        {
            Console.Write("Please try again: ");
        }

        return name;
    }

    private static TEnum ReadEnum<TEnum>(string display) where TEnum : struct
    {
        TEnum res;
        Console.Write(display);

        while (!Enum.TryParse(Console.ReadLine(), true, out res))
        {
            Console.Write("Please try again: ");
        }

        return res;
    }

    private static DateTime ReadDate(string display)
    {
        DateTime date;
        Console.Write(display);

        while (!DateTime.TryParse(Console.ReadLine(), out date))
        {
            Console.Write("Please try again: ");
        }

        return date;
    }

    private static DateTime? ReadOptionalDate(string display)
    {
        Console.WriteLine(display);
        while (Console.ReadLine() is { } input)
        {
            if (DateTime.TryParse(input, out var date))
            {
                return date;
            }

            Console.WriteLine("Please try again");
        }

        return null;
    }

    private static int ReadNumber()
    {
        int number;
        while (!int.TryParse(Console.ReadLine(), out number))
        {
            Console.WriteLine("Please try again");
        }

        return number;
    }

    private static int ReadChoice(string display, int choicesCount)
    {
        Console.WriteLine(display);
        while (true)
        {
            var choice = ReadNumber();
            if (choice >= 0 && choice < choicesCount)
            {
                return choice;
            }

            Console.WriteLine("Please try again");
        }
    }

    private static bool CreateMeeting(ref MeetingManager manager)
    {
        var name = ReadString("Meeting name: ");
        var description = ReadString("Meeting description: ");
        var responsiblePerson = ReadString("Responsible person: ");
        var category = ReadEnum<MeetingCategory>("Meeting Category (Valid: CodeMonkey | Hub | Short | TeamBuilding): ");
        var type = ReadEnum<MeetingType>("Meeting Type (Valid: Live | InPerson): ");
        var startDate = ReadDate("Meeting Start Date: ");
        var endDate = ReadDate("Meeting End Date: ");

        var meeting = new Meeting
        {
            Name = name,
            Description = description,
            ResponsiblePerson = responsiblePerson,
            MeetingCategory = category,
            MeetingType = type,
            StartDate = startDate,
            EndDate = endDate
        };
        return manager.TryAddMeeting(meeting);
    }

    private static bool DeleteMeeting(ref MeetingManager manager, string userName)
    {
        var meetingName = ReadString("Meeting name: ");
        return manager.TryRemoveMeeting(meetingName, userName);
    }

    private static bool AddToMeeting(ref MeetingManager manager)
    {
        var meetingName = ReadString("Meeting name: ");
        var personName = ReadString("Person name: ");
        var attendanceDate = ReadDate("Attendance date: ");
        var attendee = new Attendee
        {
            Name = personName,
            Date = attendanceDate
        };
        return manager.TryAddAttendeeToMeeting(meetingName, attendee);
    }

    private static bool RemoveFromMeeting(ref MeetingManager manager)
    {
        var meetingName = ReadString("Meeting name: ");
        var personName = ReadString("Person name: ");
        return manager.TryRemoveAttendeeFromMeeting(meetingName, personName);
    }

    private static void ListMeetings(IEnumerable<Meeting> meetings)
    {
        foreach (var meeting in meetings)
        {
            Console.WriteLine("-----------------------------------------\n" +
                              meeting +
                              "-----------------------------------------\n");
        }
    }

    private static List<Meeting> FilterAttendeesCount(List<Meeting> meetings)
    {
        Console.WriteLine("Enter comparison sign (< > =) and a number: ");

        List<Meeting>? filteredMeetings = null;

        do
        {
            var input = Console.ReadLine()!.Trim();

            var op = input.First();
            input = input[1..];

            if (!int.TryParse(input, out var num))
            {
                Console.WriteLine("Please try again: ");
                continue;
            }

            switch (op)
            {
                case '<':
                    filteredMeetings = meetings.FindAll(m => m.Attendees.Count < num);
                    break;
                case '>':
                    filteredMeetings = meetings.FindAll(m => m.Attendees.Count > num);
                    break;
                case '=':
                    filteredMeetings = meetings.FindAll(m => m.Attendees.Count == num);
                    break;
                default:
                    Console.WriteLine("Please try again: ");
                    break;
            }
        } while (filteredMeetings == null);

        return filteredMeetings;
    }

    private static List<Meeting> FilterName(List<Meeting> meetings)
    {
        var name = ReadString("Name: ").ToLower();
        return meetings.FindAll(m => m.Name!.ToLower().Contains(name));
    }

    private static List<Meeting> FilterDescription(List<Meeting> meetings)
    {
        var description = ReadString("Description: ").ToLower();
        return meetings.FindAll(m => m.Description!.ToLower().Contains(description));
    }

    private static List<Meeting> FilterResponsiblePerson(List<Meeting> meetings)
    {
        var personName = ReadString("Name: ").ToLower();
        return meetings.FindAll(m => m.ResponsiblePerson!.ToLower().Contains(personName));
    }

    private static List<Meeting> FilterCategory(List<Meeting> meetings)
    {
        var category = ReadEnum<MeetingCategory>("Meeting Category (Valid: CodeMonkey | Hub | Short | TeamBuilding):");
        return meetings.FindAll(m => m.MeetingCategory == category);
    }

    private static List<Meeting> FilterType(List<Meeting> meetings)
    {
        var type = ReadEnum<MeetingType>("Meeting Type (Valid: Live | InPerson)");
        return meetings.FindAll(m => m.MeetingType == type);
    }

    private static List<Meeting> FilterDate(List<Meeting> meetings)
    {
        var startDate = ReadOptionalDate("Enter Ending Date or Enter to skip");

        var endDate = startDate == null
            ? ReadDate("Enter Ending Date or Enter to skip")
            : ReadOptionalDate("Enter Ending Date or Enter to skip");

        if (startDate != null && endDate != null)
        {
            return meetings.FindAll(m => m.StartDate <= startDate && endDate <= m.EndDate);
        }

        return (startDate != null
            ? meetings.FindAll(m => m.StartDate <= startDate)
            : meetings.FindAll(m => endDate <= m.EndDate));
    }

    private static void ServeFilterMenu(MeetingManager manager)
    {
        Console.WriteLine("0. List all meetings\n" +
                          "1. Filter by meeting name\n" +
                          "2. Filter by meeting description\n" +
                          "3. Filter by responsible person\n" +
                          "4. Filter by meeting category\n" +
                          "5. Filter by meeting type\n" +
                          "6. Filter by dates\n" +
                          "7. Filter by number of attendees");

        var meetings = manager.Meetings.Values.ToList();

        switch (ReadChoice("Enter choice: ", 8))
        {
            case 0:
                break;
            case 1:
                meetings = FilterName(meetings);
                break;
            case 2:
                meetings = FilterDescription(meetings);
                break;
            case 3:
                meetings = FilterResponsiblePerson(meetings);
                break;
            case 4:
                meetings = FilterCategory(meetings);
                break;
            case 5:
                meetings = FilterType(meetings);
                break;
            case 6:
                meetings = FilterDate(meetings);
                break;
            case 7:
                meetings = FilterAttendeesCount(meetings);
                break;
        }

        ListMeetings(meetings);
    }

    public static void ServeMainMenu(ref MeetingManager manager)
    {
        var userName = ReadString("Enter your name: ");

        Console.WriteLine("0. Create new meeting\n" +
                          "1. Delete a meeting\n" +
                          "2. Add person to a meeting\n" +
                          "3. Remove person from a meeting\n" +
                          "4. List meetings");

        while (true)
        {
            var result = false;
            switch (ReadChoice("Enter choice: ", 5))
            {
                case 0:
                    result = CreateMeeting(ref manager);
                    break;
                case 1:
                    result = DeleteMeeting(ref manager, userName);
                    break;
                case 2:
                    result = AddToMeeting(ref manager);
                    break;
                case 3:
                    result = RemoveFromMeeting(ref manager);
                    break;
                case 4:
                    ServeFilterMenu(manager);
                    result = true;
                    break;
            }

            if (result)
            {
                break;
            }

            Console.WriteLine("Please try again: ");
        }
    }
}