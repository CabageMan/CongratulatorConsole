using Controllers;

namespace CongratulatorConsole;

class CongratulatorConsole
{
    static void Main(string[] args)
    {
        var controller = new MainController();
        controller.Start();

        // Connect MySQL database
    }
}
