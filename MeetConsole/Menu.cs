namespace MeetConsole;

public delegate bool MenuAction();

public delegate string MenuDisplay();

public class MenuOption
{
    public readonly MenuDisplay Display;

    public MenuAction? Action;

    public bool MenuCloses = true;

    public MenuOption(MenuDisplay display)
    {
        Display = display;
    }
}

public class Menu
{
    protected List<MenuOption> Options;
    protected MenuDisplay? Headline;

    private bool IsValidChoice(int choice)
    {
        return choice >= 0 && choice < Options.Count;
    }

    private MenuOption GetValidChoice()
    {
        Console.Write("Enter choice: ");
        int choice;
        while (!int.TryParse(Console.ReadLine(), out choice) || !IsValidChoice(choice))
        {
            Console.Write("Invalid choice, please try again: ");
        }

        return Options[choice];
    }

    protected Menu()
    {
        Options = new List<MenuOption>();
    }

    private void Display()
    {
        var headline = Headline?.Invoke();
        if (!string.IsNullOrWhiteSpace(headline))
        {
            Console.WriteLine($"---\n{headline}\n---");
        }

        for (var i = 0; i < Options.Count; ++i)
        {
            Console.WriteLine($"{i}. {Options[i].Display()}");
        }
    }

    public void Serve()
    {
        while (true)
        {
            Display();

            var choice = GetValidChoice();

            if (choice.Action != null && !choice.Action())
            {
                // Failed to apply action.
                continue;
            }

            if (choice.MenuCloses) return;
        }
    }
}

public static class MenuUtils
{
    public static string? ReadString(string fieldName)
    {
        Console.Write($"Enter {fieldName}: ");
        return Console.ReadLine();
    }

    public static DateTime? ReadDate(string fieldName)
    {
        Console.Write($"Enter {fieldName}: ");

        if (!DateTime.TryParse(Console.ReadLine(), out var date)) return null;

        return date;
    }

    public static int? ReadInt(string fieldName)
    {
        var input = ReadString(fieldName);
        if (input == null)
        {
            return null;
        }

        if (!int.TryParse(input, out var res))
        {
            return null;
        }

        return res;
    }
}

public class IntFilterSelectionMenu : Menu
{
    public Predicate<int> Predicate { get; private set; }

    public IntFilterSelectionMenu()
    {
        Options = new List<MenuOption>
        {
            new(() => "Equals")
            {
                Action = () =>
                {
                    var filterInt = MenuUtils.ReadInt("number");
                    if (filterInt == null)
                    {
                        return false;
                    }

                    Predicate = d => d == filterInt;
                    return true;
                }
            },
            new(() => "Greater Than")
            {
                Action = () =>
                {
                    var filterInt = MenuUtils.ReadInt("number");
                    if (filterInt == null)
                    {
                        return false;
                    }

                    Predicate = d => d > filterInt;
                    return true;
                }
            },
            new(() => "Less Than")
            {
                Action = () =>
                {
                    var filterInt = MenuUtils.ReadInt("number");
                    if (filterInt == null)
                    {
                        return false;
                    }

                    Predicate = d => d < filterInt;
                    return true;
                }
            }
        };
    }
}

public class DateFilterSelectionMenu : Menu
{
    public Predicate<DateTime> Predicate { get; private set; }

    public DateFilterSelectionMenu()
    {
        Options = new List<MenuOption>
        {
            new(() => "Equals")
            {
                Action = () =>
                {
                    var filterDate = MenuUtils.ReadDate("date");
                    if (filterDate == null)
                    {
                        return false;
                    }

                    Predicate = d => d == filterDate;
                    return true;
                }
            },
            new(() => "Greater Than")
            {
                Action = () =>
                {
                    var filterDate = MenuUtils.ReadDate("date");
                    if (filterDate == null)
                    {
                        return false;
                    }

                    Predicate = d => d > filterDate;
                    return true;
                }
            },
            new(() => "Less Than")
            {
                Action = () =>
                {
                    var filterDate = MenuUtils.ReadDate("date");
                    if (filterDate == null)
                    {
                        return false;
                    }

                    Predicate = d => d < filterDate;
                    return true;
                }
            }
        };
    }
}