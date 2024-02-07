using Controllers;

namespace CongratulatorConsole;

class CongratulatorConsoleApp
{
    static void Main(string[] args)
    {
        var controller = new MainController();
        controller.Start();
    }
}
