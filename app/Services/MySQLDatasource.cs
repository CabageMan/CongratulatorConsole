using MySql.Data.MySqlClient;

namespace Datasource;

public class MySQLDatasource : IDatasource
{
    public MySQLDatasource()
    {
        try
        {
            MySqlDBConnection.Instance().CreateConnection();
        }
        catch (InvalidOperationException e)
        {
            throw new InvalidOperationException("MySql Data Base Connection exception: " + e.Message);
        }
    } 

    List<RawBirthday> IDatasource.GetAllBirthdays()
    {
        MySqlDBConnection.Instance().Open();
        // Console.WriteLine("Name from DB ");
        // Console.ReadKey();
        if (MySqlDBConnection.Instance().IsOpened)
        {
            var connection = MySqlDBConnection.Instance().Connection;
            var command = new MySqlCommand
            {
                Connection = connection,
                CommandText = @"SELECT * FROM Game"
            };

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var name = reader.GetString("name");
                Console.WriteLine("Name from DB " + name);
                Console.ReadKey();
            }
        } 

        return [];
    }

    void IDatasource.AddNewBirthday(RawBirthday rawBirthday)
    {

    }

    void IDatasource.DeleteBirthdayBy(int birthdayId)
    {

    }

    void IDatasource.ReplaceBirthday(Datasource.RawBirthday rawBirthday)
    {

    }
}