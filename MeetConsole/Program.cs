using MeetConsole.Controllers;
using MeetConsole.Services;

namespace MeetConsole;

public static class Program
{
    public static void Main()
    {
        var manager = new MeetingManager(new MeetingRepository());
        var mainMenuController = new MainMenuController(manager);
        mainMenuController.Control();
    }
}