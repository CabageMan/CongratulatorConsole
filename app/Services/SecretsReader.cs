using CongratulatorConsole;
using Microsoft.Extensions.Configuration;

namespace SecretsReader;

public class SecretsSettingsReader
{
    public static T? ReadSection<T>(string sectionName)
    {
        var environment = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");
        var builder = new ConfigurationBuilder()
            .AddJsonFile("appSettings.json")
            .AddJsonFile($"appSettings.{environment}.json", optional: true)
            .AddUserSecrets<CongratulatorConsoleApp>(false)
            .AddEnvironmentVariables()
            .Build();

        return builder.GetSection(sectionName).Get<T>();
    }
}

public class SecretValues
{
    public string Username { get; set; }

    public string Password { get; set; }
}