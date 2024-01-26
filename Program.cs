using Controllers;

namespace CongratulatorConsole;

class Program
{
    static void Main(string[] args)
    {
        var controller = new MainController();
        controller.Start();

        // Connect files write and read from it.
        // Connect MySQL database
    }
}
