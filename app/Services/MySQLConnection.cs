using MySql.Data.MySqlClient;

namespace Datasource;

public class MySqlDBConnection
{
    private static MySqlDBConnection? _instance = null;

    // Get from environment variable
    private string DB_Host = "localhost";
    private string DB_Port = "3306";
    private string DB_Name = "";
    private string DB_UserName = "";
    private string DB_Password = "";
    public MySqlConnection Connection { get; private set; }

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

                cmd.CommandText = "CREATE TABLE IF NOT EXISTS `Birthday` (" +
                    "`id` INT AUTO_INCREMENT," +
                    "`role` TEXT," +
                    "`first_name` TEXT," +
                    "`last_name` TEXT," +
                    "`birth_date` DATE," +
                    "PRIMARY KEY(id));"
                ;
                cmd.ExecuteNonQuery();
                Connection.Close();

                Connection = new MySqlConnection(GetConnectionString(true));
            }
            catch (MySqlException e)
            {
                throw new InvalidOperationException("Could not create connection: " + e.Message);
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
        Connection.Close();
    }

    public bool IsOpened
    {
        get => Connection != null && Connection.State == System.Data.ConnectionState.Open;
    }

    // TODO: Check for more connection options.
    private string GetConnectionString(bool withDbName)
    {
        return withDbName ?
            string.Format($"server={DB_Host}; port={DB_Port}; database={DB_Name}; uid={DB_UserName}; pwd={DB_Password}") :
            string.Format($"server={DB_Host}; port={DB_Port}; uid={DB_UserName}; pwd={DB_Password}");
    }
}

