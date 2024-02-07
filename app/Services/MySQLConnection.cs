using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Datasource;

public class MySqlDBConnection
{
    private static MySqlDBConnection? _instance = null;

    // Get from environment variable
    private const string DB_HOST = "localhost";
    private const string DB_PORT = "3306";
    private const string DB_NAME = "congratulator_birthdays";
    private readonly string? dbUserName;
    private readonly string? dbPassword;
    public MySqlConnection? Connection { get; private set; }

    public MySqlDBConnection()
    {
        IConfigurationRoot config = new ConfigurationBuilder()
            .AddUserSecrets<MySqlDBConnection>()
            .Build();
        dbUserName = config["CongratulatorDataBase:UserName"];
        dbPassword = config["CongratulatorDataBase:Password"];
        // Console.WriteLine($"Name: {dbUserName}, Password: {dbPassword}");
        // Console.ReadKey();
    }

    public static MySqlDBConnection Instance()
    {
        if (_instance == null)
        {
            _instance = new MySqlDBConnection();
        }
        return _instance;
    }

    public void CreateConnection()
    {
        if (Connection == null)
        {
            Connection = new MySqlConnection(GetConnectionString(false));
            try
            {
                Connection.Open();
                var cmd = new MySqlCommand
                {
                    Connection = Connection,
                    CommandText = "CREATE DATABASE IF NOT EXISTS `congratulator_birthdays`;"
                };
                cmd.ExecuteNonQuery();
                Connection.Close();

            }
            catch (MySqlException e)
            {
                throw new InvalidOperationException("Could not create DataBase: " + e.Message);
            }

            Connection = new MySqlConnection(GetConnectionString(true));
            try
            {
                Connection.Open();
                var cmd = new MySqlCommand
                {
                    Connection = Connection,
                    CommandText = "CREATE TABLE IF NOT EXISTS `Birthday` (" +
                        "`id` INT AUTO_INCREMENT," +
                        "`role` TEXT," +
                        "`first_name` TEXT," +
                        "`last_name` TEXT," +
                        "`birth_date` DATE," +
                        "PRIMARY KEY(id));"
                };
                cmd.ExecuteNonQuery();
                Connection.Close();
            }
            catch (MySqlException e)
            {
                throw new InvalidOperationException("Could not create table: " + e.Message);
            }
        }
    }

    public void Open()
    {
        if (Connection != null)
        {
            Connection.Open();
        }
        else
        {
            throw new InvalidOperationException("Could not open: connection is not established.");
        }
    }

    public void Close()
    {
        if (Connection != null)
        {
            Connection.Close();
        }
    }

    public bool IsOpened
    {
        get => Connection != null && Connection.State == System.Data.ConnectionState.Open;
    }

    // TODO: Check for more connection options.
    private string GetConnectionString(bool withDbName)
    {
        return withDbName ?
            string.Format($"server={DB_HOST}; port={DB_PORT}; database={DB_NAME}; uid={dbUserName}; pwd={dbPassword}") :
            string.Format($"server={DB_HOST}; port={DB_PORT}; uid={dbUserName}; pwd={dbPassword}");
    }
}

