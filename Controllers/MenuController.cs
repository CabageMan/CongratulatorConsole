using Microsoft.VisualBasic;
using Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Controllers;

public class MenuController 
{
    private const ConsoleColor TITLE_COLOR = ConsoleColor.Green;
    private const ConsoleColor ITEM_COLOR = ConsoleColor.Blue;
    private const ConsoleColor ERROR_COLOR = ConsoleColor.Red;

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

    public void Start() 
    {
        Console.Clear();
        while(!exitApp)
        {
            ShowMainMenu();
        }
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
        // Console.Clear();
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
                Console.WriteLine("Show all birthdays");
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

    private static void HandleAddingNewUser(UserRole userRole) {
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

        // Temp
        Console.WriteLine($"A new {userRole} {userFirstName} {userLastName} was born on {userBirthDate.ToString("MM-dd-yyyy")}");
    }
}