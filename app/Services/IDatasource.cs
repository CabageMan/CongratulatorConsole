using Models;

namespace Datasource;

public interface IDatasource
{
    List<BirthdayPerson> GetAllBirthdays();
    long AddNewBirthday(BirthdayPerson brthday);
    void DeleteBirthdayBy(long birthdayId);
    void ReplaceBirthday(BirthdayPerson brthday);
}

