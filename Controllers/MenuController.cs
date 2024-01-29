using Microsoft.VisualBasic;
using Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Controllers;

public class MenuController(
    Action showBitrhdays,
    Action<string, string, UserRole, DateOnly> addNewUser)
{
    private const ConsoleColor TITLE_COLOR = ConsoleColor.Green;
    private const ConsoleColor ITEM_COLOR = ConsoleColor.Blue;
    private const ConsoleColor ERROR_COLOR = ConsoleColor.Red;
    private const ConsoleColor WARNING_COLOR = ConsoleColor.Yellow;

    enum MainMenuItem 
    {
        ShowAll = 1,
        ShowUpcoming,
        AddNew,
        Delete,
        Edit, 
        Exit
    }

    private bool exitApp = false;

    private readonly Action _showBitrhdays = showBitrhdays;
    private readonly Action<string, string, UserRole, DateOnly> _addNewUser = addNewUser;

    public void Start() 
    {
        Console.Clear();
        while(!exitApp)
        {
            ShowMainMenu();
        }
    }

    public static void ShowAllBirthdays(List<BirthdayUser> birthdayUsers) 
    {
        if (birthdayUsers.Count != 0) {
            DrawBirthdaysTable(birthdayUsers);
        } else {
            Console.ForegroundColor = WARNING_COLOR;
            Console.WriteLine("There are no birthdays records yet", Console.ForegroundColor);
            Console.ForegroundColor = ITEM_COLOR;
        }

        var userInput = "";
        do {
            Console.Write("Type \"x\" to back to the main menu\n> ");
            userInput = Console.ReadLine();
        } while(userInput != "x");
        Console.Clear();
    }

    // Show menu 
    private void ShowMainMenu() 
    {
        ShowTitle("Enter a number of menu item:");
        ShowMenuItems<MainMenuItem>();
        HandleSelectedMainMenuItem(GetUsersMenuItemInput<MainMenuItem>());
    }

    private void ShowAddUserMenu() 
    {
        Console.Clear();
        ShowTitle("Select user role:");
        ShowMenuItems<UserRole>();
        HandleAddingNewUser(GetUsersMenuItemInput<UserRole>());
    }

    private static void ShowTitle(string title)
    {
        Console.ForegroundColor = TITLE_COLOR;
        Console.WriteLine(title + Environment.NewLine, Console.ForegroundColor);
    }

    private static void ShowMenuItems<E>() where E: Enum 
    {
        Console.ForegroundColor = ITEM_COLOR;
        foreach(int enumItem in Enum.GetValues(typeof(E))) 
        {
            Console.Write($"\t{enumItem} - ");
            Console.WriteLine($"{Enum.GetName(typeof(E), enumItem)}");
        }
        Console.Write(Environment.NewLine);
    }

    // Improve drawing data! 
    private static void DrawBirthdaysTable(List<BirthdayUser> birthdayUsers) 
    {
        var titlesWithSpaces = new(string title, int space) [] { 
            ("ID", 4), 
            ("Role", 8), 
            ("Name", 10), 
            ("Birth Date", 10) 
        };
        DrawTableHeader(titlesWithSpaces);

        foreach(BirthdayUser user in birthdayUsers) {
            // Console.WriteLine(
            //     $"{user.Id}{Tabs(4-user.Id.ToString().Length)} | " + 
            //     $"{user.Role}{Tabs(8 - user.Role.ToString().Length)} | " +
            //     $"{user.FullName}{Tabs(10 - user.FullName.Length)} | " +
            //     $"{user.BirthDate:MM-dd-yyyy} | "
            // );
            Console.WriteLine(
                $"{user.Id} | " + 
                $"{user.Role} | " +
                $"{user.FullName} | " +
                $"{user.BirthDate:MM-dd-yyyy} |"
            );
        }
    }

    private static void DrawTableHeader((string, int)[] titlesWithSpaces) 
    {
        Console.ForegroundColor = WARNING_COLOR;
        foreach((string title, int space) in titlesWithSpaces)
        {
            var whiteSpace = Tabs(space - title.Length);
            Console.Write($"{title}{whiteSpace} | ");
        }
        Console.Write(Environment.NewLine);
        Console.ForegroundColor = ITEM_COLOR;
    }

    // Get user input
    private static E GetUsersMenuItemInput<E>() where E: Enum
    {
        int menuItemsCount = Enum.GetValues(typeof(E)).Length;
        Console.Write("> ");
        var userInput = Console.ReadLine();
        var menuItemNumber = menuItemsCount;

        var allCases = (IEnumerable<int>)Enum.GetValues(typeof(E));
        var minValue = allCases.ToArray().Min();

        while(!int.TryParse(userInput, out menuItemNumber) || menuItemNumber > menuItemsCount || menuItemNumber < minValue)
        {
            Console.ForegroundColor = ERROR_COLOR;
            Console.WriteLine($"There is no menu item with number {menuItemNumber}.{Environment.NewLine}Please enter number between {minValue} and {menuItemsCount}", Console.ForegroundColor);            
            Console.ForegroundColor = ITEM_COLOR;
            Console.Write("> ");
            userInput = Console.ReadLine();
        }

        return (E)Enum.ToObject(typeof(E), menuItemNumber);
    }

    // Handle User Input
    private void HandleSelectedMainMenuItem(MainMenuItem item) 
    {
        Console.Clear();

        switch (item) 
        {
            case MainMenuItem.ShowAll:
                _showBitrhdays();
                break;
            case MainMenuItem.ShowUpcoming:
                Console.WriteLine("Show all upcoming birthdays");
                break;
            case MainMenuItem.AddNew:
                ShowAddUserMenu();
                break;
            case MainMenuItem.Delete:
                Console.WriteLine("Delete birthday");
                break;
            case MainMenuItem.Edit:
                Console.WriteLine("Edit birthday");
                break;
            case MainMenuItem.Exit:
                exitApp = true;
                Console.WriteLine("Exiting the application...");
                break;
        }
    }

    private void HandleAddingNewUser(UserRole userRole) {
        Console.Clear();
        Console.WriteLine($"Enter new {userRole} first name:{Environment.NewLine}");
        Console.Write("> ");
        var userFirstName = Console.ReadLine();

        Console.WriteLine($"Enter {userFirstName}'s last name:{Environment.NewLine}");
        Console.Write("> ");
        var userLastName = Console.ReadLine();

        Console.WriteLine($"Enter {userFirstName}'s {userLastName} bith date in format MM/dd/yyyy:{Environment.NewLine}");
        Console.Write("> ");
        var userBirthDateString = Console.ReadLine();

        var userBirthDate = DateOnly.FromDateTime(DateTime.Now);
        while(!DateOnly.TryParse(userBirthDateString, out userBirthDate)) {
            Console.ForegroundColor = ERROR_COLOR;
            Console.WriteLine($"Date input is incorrect {userBirthDateString}.{Environment.NewLine}Please enter correct date in format MM/dd/yyyy", Console.ForegroundColor);            
            Console.ForegroundColor = ITEM_COLOR;
            Console.Write("> ");
            userBirthDateString = Console.ReadLine();
        }

        _addNewUser(userFirstName ?? "", userLastName ?? "", userRole, userBirthDate);

        Console.Clear();
    }

    // Helpers
    private static string Tabs(int n)
    {
        return new string('\t', n);
    }

    // private static int GetLargestLength(string[] strings)
    // {
    //     return new string('\t', n);
    // }

    // private static int GetCharCount(int number)
    // {
    //     return GetCharCount(number.ToString());
    // }

    // private static int GetCharCount(string text)
    // {
    //     return text.Length;
    // }
}