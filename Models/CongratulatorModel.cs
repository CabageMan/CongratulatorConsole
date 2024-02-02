using System.Runtime.Serialization;
using Datasource;

namespace Models;

public class CongratulatorModel
{
    private readonly IDatasource datasource;
    public List<BirthdayUser> BirthdayUsers { get; set; }

    public CongratulatorModel()
    {
        datasource = new FileDatasource();
        BirthdayUsers = ConvertFromRawBirthdays(datasource.GetAllBirthdays());
    }

    // Could throw an error if validation or adding is failed
    public bool AddNewBirthday(UserRole role, string firstName, string lastName, DateOnly birthDate)
    {
        // Add validations if names empty, role is wrong and birthday is in future in controller
        int lastUserId = BirthdayUsers.Count == 0 ? 0 : BirthdayUsers.Last().Id;
        BirthdayUser newUser = new(++lastUserId, firstName, lastName, birthDate, role);
        BirthdayUsers.Add(newUser);

        datasource.AddNewBirthday(new RawBirthday(
            newUser.Id,
            newUser.Role.ToString(),
            newUser.FirstName,
            newUser.LastName,
            newUser.BirthDateString
            ));

        return true;
    }

    // Could throw an error if deletion is failed
    public bool DeleteBirthdayBy(int birthdayId)
    {
        // Handle exceptions
        BirthdayUsers.RemoveAll(user => user.Id == birthdayId);
        datasource.DeleteBirthdayBy(birthdayId);
        return true;
    }

    public bool EditBirthday(BirthdayUser editedBirthdayUser)
    {
        // Handle exceptions
        Console.WriteLine($"User Updated: {editedBirthdayUser.ToString()}");
        int editedUserIndex = BirthdayUsers.FindIndex(user => user.Id == editedBirthdayUser.Id);
        BirthdayUsers[editedUserIndex] = editedBirthdayUser;
        datasource.ReplaceBirthday(new RawBirthday(
            editedBirthdayUser.Id,
            editedBirthdayUser.Role.ToString(),
            editedBirthdayUser.FirstName,
            editedBirthdayUser.LastName,
            editedBirthdayUser.BirthDateString
        ));

        return true;
    }

    private static List<BirthdayUser> ConvertFromRawBirthdays(List<RawBirthday> rawBirthdays)
    {
        List<BirthdayUser> birthdayUsers = [];
        foreach (RawBirthday rawBirthday in rawBirthdays)
        {
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
                throw new InvalidOperationException("Could not parse role or date");
            }
        }
        return birthdayUsers;
    }
}