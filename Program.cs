using System.ComponentModel;
using Microsoft.VisualBasic;

namespace CongratulatorConsole;

class Program
{
    static void Main(string[] args)
    {
        var controller = new MenuController();
        controller.Start();

        // Connect files write and read from it.
        // Connect MySQL database
    }
}

class MenuController 
{
    private const ConsoleColor TITLE_COLOR = ConsoleColor.Green;
    private const ConsoleColor ITEM_COLOR = ConsoleColor.Blue;
    private const ConsoleColor ERROR_COLOR = ConsoleColor.Red;

    enum MenuItem 
    {
        ShowAll = 1,
        ShowUpcoming = 2,
        AddNew = 3,
        Delete = 4,
        Edit = 5, 
        Exit = 6
    }

    private bool exitApp = false;

    public void Start() 
    {
        while(!exitApp)
        {
            ShowMenu();
            HandleSelectedItem(GetUserNumberInput());
        }
    }

    private static void ShowMenu() 
    {
        // Show title:
        Console.ForegroundColor = TITLE_COLOR;
        Console.WriteLine($"Enter a number of menu item:{Environment.NewLine}", Console.ForegroundColor);

        Console.ForegroundColor = ITEM_COLOR;
        foreach(int menuItem in Enum.GetValues(typeof(MenuItem))) 
        {
            Console.Write($"\t{menuItem} - ");
            Console.WriteLine($"{Enum.GetName(typeof(MenuItem), menuItem)}");
        }
        Console.Write(Environment.NewLine);
    }

    private static MenuItem GetUserNumberInput() 
    {
        int menuItemsCount = Enum.GetValues(typeof(MenuItem)).Length;
        var userInput = Console.ReadLine();
        var menuItemNumber = menuItemsCount;

        while(!int.TryParse(userInput, out menuItemNumber) || menuItemNumber > menuItemsCount || menuItemNumber <= 0)
        {
            Console.ForegroundColor = ERROR_COLOR;
            Console.WriteLine($"There is no menu item with number {menuItemNumber}.{Environment.NewLine}Please enter number between ...", Console.ForegroundColor);            
            Console.ForegroundColor = ITEM_COLOR;
            userInput = Console.ReadLine();
        }

        return (MenuItem)Enum.ToObject(typeof(MenuItem), menuItemNumber);
    }

    private void HandleSelectedItem(MenuItem item) 
    {
        Console.Clear();

        switch (item) 
        {
            case MenuItem.ShowAll:
                Console.WriteLine("Show all birthdays");
                break;
            case MenuItem.ShowUpcoming:
                Console.WriteLine("Show all upcoming birthdays");
                break;
            case MenuItem.AddNew:
                Console.WriteLine("Add new birthday");
                break;
            case MenuItem.Delete:
                Console.WriteLine("Delete birthday");
                break;
            case MenuItem.Edit:
                Console.WriteLine("Edit birthday");
                break;
            case MenuItem.Exit:
                exitApp = true;
                Console.WriteLine("Exiting the application...");
                break;
        }
    }
}