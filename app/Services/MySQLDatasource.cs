

namespace Datasource;

public class MySQLDatasource : IDatasource
{

    List<RawBirthday> IDatasource.GetAllBirthdays()
    {
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