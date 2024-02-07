using Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SecretsReader;

namespace CongratulatorConsole;

class CongratulatorConsoleApp
{
    static void Main(string[] args)
    {
        // var secretValues = SecretsSettingsReader.ReadSection<SecretValues>("CongratulatorDatabase");
        // var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
        // Console.WriteLine("Name is " + secretValues.Username);

        var controller = new MainController();
        controller.Start();
    }
}
