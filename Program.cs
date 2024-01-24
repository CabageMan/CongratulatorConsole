namespace CongratulatorConsole;

class Program
{
    static void Main(string[] args)
    {
        var nameConstant = " name";
        Console.WriteLine("Hello!\nWhat is yout {0}?", nameConstant);
        var userName = Console.ReadLine();
        var currentDate = DateTime.Now;
        Console.WriteLine($"{Environment.NewLine}Hello, {userName}, on {currentDate:dddd} at {currentDate:T}");
        Console.Write($"{Environment.NewLine}Press any key to exit ...");
        Console.ReadKey(false);
    }
}
