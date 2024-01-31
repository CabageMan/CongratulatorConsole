using System.Runtime.Serialization;

namespace Models;

public class CongratulatorModel 
{
    public List<BirthdayUser> BirthdayUsers { get; set; }

    public CongratulatorModel() {
        // Init here a data manager (files or data base)
        BirthdayUsers = [];
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
}