using MeetConsole.Interfaces;
using MeetConsole.Models;
using MeetConsole.Services;
using MeetConsole.UI;

namespace MeetConsole.Controllers;

public class FilterMenuController : IController
{
    private List<Meeting> _meetings;

    private void FilterByName()
    {
        var personName = MeetingUi.ReadString("Person name: ");
        _meetings = MeetingFilter.FilterByName(_meetings, personName);
    }

    private void FilterByDescription()
    {
        var description = MeetingUi.ReadString("Meeting description: ");
        _meetings = MeetingFilter.FilterByDescription(_meetings, description);
    }

    private void FilterByResponsiblePerson()
    {
        var personName = MeetingUi.ReadString("Person name: ");
        _meetings = MeetingFilter.FilterByResponsiblePerson(_meetings, personName);
    }

    private void FilterByCategory()
    {
        var category = MeetingUi.ReadEnum<MeetingCategory>(
            "Meeting Category (Valid: CodeMonkey | Hub | Short | TeamBuilding): ");
        _meetings = MeetingFilter.FilterByCategory(_meetings, category);
    }

    private void FilterByType()
    {
        var type = MeetingUi.ReadEnum<MeetingType>("Meeting Type (Valid: Live | InPerson): ");
        _meetings = MeetingFilter.FilterByType(_meetings, type);
    }

    private void FilterByDate()
    {
        DateInterval interval;

        interval.Start = MeetingUi.ReadOptionalDate("Enter Ending Date or Enter to skip");

        interval.End = interval.Start == null
            ? MeetingUi.ReadDate("Enter Ending Date or Enter to skip")
            : MeetingUi.ReadOptionalDate("Enter Ending Date or Enter to skip");

        _meetings = MeetingFilter.FilterByDate(_meetings, interval);
    }

    private void FilterByAttendeesCount()
    {
        Console.WriteLine("Enter comparison sign (< > =) and a number: ");

        char op;
        NumberFilter<int> filter;

        do
        {
            var input = Console.ReadLine()!.Trim();

            op = input.First();
            input = input[1..];

            if (!int.TryParse(input, out filter.Number))
            {
                Console.WriteLine("Please try again: ");
                continue;
            }

            if (op != '<' && op != '>' && op != '=')
            {
                Console.WriteLine("Please try again: ");
                continue;
            }

            break;
        } while (op != '<' && op != '>' && op != '=');

        filter.Comparison = (NumberComparison)op;

        _meetings = MeetingFilter.FilterByAttendeesCount(_meetings, filter);
    }

    public FilterMenuController(List<Meeting> meetings)
    {
        _meetings = meetings;
    }

    public void Control()
    {
        MeetingUi.DisplayFilterMenu();

        while (true)
        {
            var choice = (FilterMenuChoice)MeetingUi.ReadNumber();
            var valid = true;

            switch (choice)
            {
                case FilterMenuChoice.ListMeetings:
                    break;
                case FilterMenuChoice.FilterName:
                    FilterByName();
                    break;
                case FilterMenuChoice.FilterDescription:
                    FilterByDescription();
                    break;
                case FilterMenuChoice.FilterResponsiblePerson:
                    FilterByResponsiblePerson();
                    break;
                case FilterMenuChoice.FilterMeetingCategory:
                    FilterByCategory();
                    break;
                case FilterMenuChoice.FilterMeetingType:
                    FilterByType();
                    break;
                case FilterMenuChoice.FilterDates:
                    FilterByDate();
                    break;
                case FilterMenuChoice.FilterAttendeesCount:
                    FilterByAttendeesCount();
                    break;
                default:
                    valid = false;
                    Console.WriteLine("Invalid choice");
                    Console.WriteLine("Try again: ");
                    break;
            }

            if (!valid) continue;
            MeetingUi.ListMeetings(_meetings);
            break;
        }
    }
}