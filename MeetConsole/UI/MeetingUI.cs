using MeetConsole.Models;

namespace MeetConsole.UI;

public static class MeetingUi
{
    public static string ReadString(string display)
    {
        string name;
        Console.Write(display);

        while ((name = Console.ReadLine()!).Length == 0)
        {
            Console.Write("Please try again: ");
        }

        return name;
    }

    public static TEnum ReadEnum<TEnum>(string display) where TEnum : struct
    {
        TEnum res;
        Console.Write(display);

        while (!Enum.TryParse(Console.ReadLine(), true, out res))
        {
            Console.Write("Please try again: ");
        }

        return res;
    }

    public static DateTime ReadDate(string display)
    {
        DateTime date;
        Console.Write(display);

        while (!DateTime.TryParse(Console.ReadLine(), out date))
        {
            Console.Write("Please try again: ");
        }

        return date;
    }

    public static DateTime? ReadOptionalDate(string display)
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

    public static int ReadNumber()
    {
        int number;
        while (!int.TryParse(Console.ReadLine(), out number))
        {
            Console.WriteLine("Please try again");
        }

        return number;
    }

    public static void ListMeetings(IEnumerable<Meeting> meetings)
    {
        foreach (var meeting in meetings)
        {
            Console.WriteLine("-----------------------------------------\n" +
                              meeting +
                              "-----------------------------------------\n");
        }
    }
    
    public static void DisplayFilterMenu()
    {
        Console.WriteLine("0. List all meetings\n" +
                          "1. Filter by meeting name\n" +
                          "2. Filter by meeting description\n" +
                          "3. Filter by responsible person\n" +
                          "4. Filter by meeting category\n" +
                          "5. Filter by meeting type\n" +
                          "6. Filter by dates\n" +
                          "7. Filter by number of attendees");
    }

    public static void DisplayMainMenu()
    {
        Console.WriteLine("0. Create a new meeting\n" +
                          "1. Delete a meeting\n" +
                          "2. Add person to a meeting\n" +
                          "3. Remove person from a meeting\n" +
                          "4. List meetings");
    }
}