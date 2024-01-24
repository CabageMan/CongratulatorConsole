namespace CongratulatorConsole;

class Program
{

    enum MenuItems 
    {
        ShowAll = 1,
        ShowUpcoming = 2,
        AddNew = 3,
        Delete = 4,
        Edit = 5
    }

    static void Main(string[] args)
    {
        // var nameConstant = "name";
        // Console.WriteLine("Hello!\nWhat is yout {0}?", nameConstant);
        // var userName = Console.ReadLine();
        // var currentDate = DateTime.Now;
        // Console.WriteLine($"{Environment.NewLine}Hello, {userName}, on {currentDate:dddd} at {currentDate:T}");
        // Console.Write($"{Environment.NewLine}Press any key to exit ...");
        // Console.ReadKey(false);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Enter the number of menu item: ", Console.ForegroundColor);
        Console.ForegroundColor = ConsoleColor.Blue;

        foreach(int menuItem in Enum.GetValues(typeof(MenuItems))) 
        {
            Console.Write($"{Enum.GetName(typeof(MenuItems), menuItem)}");
            Console.WriteLine($" {menuItem}");
        }

        Console.WriteLine($"Selected number is: {Console.ReadLine()}");
        Console.ReadKey(true);
    }
}
