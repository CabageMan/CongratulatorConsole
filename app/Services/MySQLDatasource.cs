using System.Data;
using MySql.Data.MySqlClient;

namespace Datasource;

public class MySQLDatasource : IDatasource
{
    public const string BIRTHDAY_TABLE_NAME = "Birthday";
    public const string ID_COLUMN_NAME = "id";
    public const string ROLE_COLUMN_NAME = "role";
    public const string FIRST_NAME_COLUMN_NAME = "first_name";
    public const string LAST_NAME_COLUMN_NAME = "last_name";
    public const string BIRTH_DATE_COLUMN_NAME = "birth_date";
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
        try
        {
            List<RawBirthday> rawBirthdays = [];
            MySqlDBConnection.Instance().Open();
            if (MySqlDBConnection.Instance().IsOpened)
            {
                var connection = MySqlDBConnection.Instance().Connection;
                var command = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT * FROM Birthday"
                };

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var id = reader.GetInt32(ID_COLUMN_NAME);
                    var roleString = reader.GetString(ROLE_COLUMN_NAME);
                    var firstName = reader.GetString(FIRST_NAME_COLUMN_NAME);
                    var lastName = reader.GetString(LAST_NAME_COLUMN_NAME);
                    var birthDate = DateOnly.FromDateTime(reader.GetDateTime(BIRTH_DATE_COLUMN_NAME));

                    rawBirthdays.Add(new(id, roleString, firstName, lastName, birthDate));
                }
                MySqlDBConnection.Instance().Close();
            }

            return rawBirthdays;
        }
        catch (InvalidOperationException e)
        {
            throw new InvalidOperationException(e.Message);
        }
        catch (MySqlException e)
        {
            throw new InvalidOperationException(e.Message);
        }
    }

    void IDatasource.AddNewBirthday(RawBirthday rawBirthday)
    {
    }

    void IDatasource.DeleteBirthdayBy(int birthdayId)
    {

    }

    void IDatasource.ReplaceBirthday(Datasource.RawBirthday rawBirthday)
    {
        string query = "UPDATE " + BIRTHDAY_TABLE_NAME + " SET " + ROLE_COLUMN_NAME +
            "=@RoleValue, " + FIRST_NAME_COLUMN_NAME + "=@FirstNameValue, " +
            LAST_NAME_COLUMN_NAME + "=@LastNameValue, " + BIRTH_DATE_COLUMN_NAME +
            "=@BirthDateValue WHERE " + ID_COLUMN_NAME + "=@IdValue";

        try
        {
            MySqlDBConnection.Instance().Open();
            if (MySqlDBConnection.Instance().IsOpened)
            {
                var cmd = new MySqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@RoleValue", rawBirthday.RoleString);
                cmd.Parameters.AddWithValue("@FirstNameValue", rawBirthday.FirstName);
                cmd.Parameters.AddWithValue("@LastNameValue", rawBirthday.LastName);
                cmd.Parameters.AddWithValue("@BirthDateValue", rawBirthday.BirthDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@IdValue", rawBirthday.Id);
                cmd.Connection = MySqlDBConnection.Instance().Connection;
                cmd.ExecuteNonQuery();
                MySqlDBConnection.Instance().Close();
            }
        }
        catch (InvalidOperationException e)
        {
            throw new InvalidOperationException(e.Message);
        }
        catch (MySqlException e)
        {
            throw new InvalidOperationException(e.Message);
        }
    }
}