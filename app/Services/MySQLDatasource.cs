using System.Data;
using System.Runtime.CompilerServices;
using Models;
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

    List<BirthdayPerson> IDatasource.GetAllBirthdays()
    {
        try
        {
            List<BirthdayPerson> birthdays = [];
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
                    long id = reader.GetInt64(ID_COLUMN_NAME);
                    var roleString = reader.GetString(ROLE_COLUMN_NAME);
                    Enum.TryParse(roleString, true, out PersonRole role);
                    var firstName = reader.GetString(FIRST_NAME_COLUMN_NAME);
                    var lastName = reader.GetString(LAST_NAME_COLUMN_NAME);
                    var dateString = reader.GetDateTime(BIRTH_DATE_COLUMN_NAME);
                    var birthDate = DateOnly.FromDateTime(dateString);

                    birthdays.Add(new(id, firstName, lastName, birthDate, role));
                }
                MySqlDBConnection.Instance().Close();
            }

            return birthdays;
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

    long IDatasource.AddNewBirthday(BirthdayPerson birthday)
    {
        try
        {
            MySqlDBConnection.Instance().Open();
            if (MySqlDBConnection.Instance().IsOpened)
            {
                string query = "INSERT INTO " + BIRTHDAY_TABLE_NAME + " (" + 
                    ROLE_COLUMN_NAME + ", " + FIRST_NAME_COLUMN_NAME + ", " +
                    LAST_NAME_COLUMN_NAME + ", " + BIRTH_DATE_COLUMN_NAME + ") " +
                    "VALUES (@RoleValue, @FirstNameValue, @LastNameValue, @BirthDateValue)";
                var cmd = new MySqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@RoleValue", birthday.RoleString);
                cmd.Parameters.AddWithValue("@FirstNameValue", birthday.FirstName);
                cmd.Parameters.AddWithValue("@LastNameValue", birthday.LastName);
                cmd.Parameters.AddWithValue("@BirthDateValue", birthday.BirthDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@IdValue", birthday.Id);
                cmd.Connection = MySqlDBConnection.Instance().Connection;
                cmd.ExecuteNonQuery();
                long id = cmd.LastInsertedId;
                MySqlDBConnection.Instance().Close();
                return id;
            }
            else {
                throw new InvalidOperationException("Could not open connection");
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

    void IDatasource.DeleteBirthdayBy(long birthdayId)
    {

    }

    void IDatasource.ReplaceBirthday(BirthdayPerson rawBirthday)
    {
        try
        {
            MySqlDBConnection.Instance().Open();
            if (MySqlDBConnection.Instance().IsOpened)
            {
                string query = "UPDATE " + BIRTHDAY_TABLE_NAME + " SET " + ROLE_COLUMN_NAME +
                    "=@RoleValue, " + FIRST_NAME_COLUMN_NAME + "=@FirstNameValue, " +
                    LAST_NAME_COLUMN_NAME + "=@LastNameValue, " + BIRTH_DATE_COLUMN_NAME +
                    "=@BirthDateValue WHERE " + ID_COLUMN_NAME + "=@IdValue";
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