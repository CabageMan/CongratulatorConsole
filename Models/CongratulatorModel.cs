using System.Runtime.Serialization;
using Datasource;

namespace Models;

public class CongratulatorModel 
{
    private IDatasource datasource;
    public List<BirthdayUser> BirthdayUsers { get; set; }

    public CongratulatorModel() {
        datasource = new FileDatasource();
        BirthdayUsers = ConvertFromRawBirthdays(datasource.GetAllBirthdayUsers());
    }

    // Could throw an error if validation or adding is failed
    public bool AddNewBirthday(UserRole role, string firstName, string lastName, DateOnly birthDate) 
    {
        // Add validations if names empty, role is wrong and birthday is in future in controller
        int lastUserId = BirthdayUsers.Count == 0 ? 0 : BirthdayUsers.Last().Id;
        BirthdayUsers.Add(new(++lastUserId, firstName, lastName, birthDate, role));
        return true;
    }

    // Could throw an error if deletion is failed
    public bool DeleteBirthdayBy(int birthdayId)
    {
        // Find index or element and if it exists remove from class.
        // var userToDeleteIndex = BirthdayUsers.FindIndex()
        // if (BirthdayUsers.Select(user => user.Id).Contains(userId)) {

        // }
        datasource.PutAllBirthdayUsers([]);
        BirthdayUsers.RemoveAll(user => user.Id == birthdayId);
        return true;
    }

    public bool EditBirthday(BirthdayUser editedBirthdayUser)
    {
        // Find index or element and if it exists remove from class.
        // var userToDeleteIndex = BirthdayUsers.FindIndex()
        // if (BirthdayUsers.Select(user => user.Id).Contains(userId)) {

        // }
        Console.WriteLine($"User Updated: {editedBirthdayUser.ToString()}");

        return true;
    }

    private static List<BirthdayUser> ConvertFromRawBirthdays(List<RawBirthday> rawBirthdays)
    {
        List<BirthdayUser> birthdayUsers = [];
        foreach (RawBirthday rawBirthday in rawBirthdays) {
            if (
                Enum.TryParse(rawBirthday.RoleString, true, out UserRole role) &&
                DateOnly.TryParse(rawBirthday.BirthDateString, out DateOnly birthDate))
            {
                birthdayUsers.Add(new(
                    rawBirthday.Id,
                    rawBirthday.FirstName,
                    rawBirthday.LastName,
                    birthDate,
                    role)
                );
            }
            else
            {
                throw new Exception();
            }
        }
        return birthdayUsers;

        /*
        return rawBirthdays.Select(rawBirthday => {
            Enum.TryParse(rawBirthday.RoleString, out UserRole userRole);
            DateOnly.TryParse(rawBirthday.BirthDateString, out DateOnly birthDate);
            return new BirthdayUser(rawBirthday.Id, rawBirthday.FirstName, rawBirthday.LastName, birthDate, userRole);
        }).ToList();
        */
    }
}