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
    public bool AddNewUser(UserRole role, string firstName, string lastName, DateOnly birthDate) 
    {
        // Add validations if names empty, role is wrong and birthday is in future in controller
        int lastUserId = BirthdayUsers.Count == 0 ? 1 : BirthdayUsers.Last().Id;
        BirthdayUsers.Add(new(++lastUserId, firstName, lastName, birthDate, role));
        return true;
    }

    // Could throw an error if deletion is failed
    public bool DeleteUser(int userId)
    {
        // Find index or element and if it exists remove from class.
        // var userToDeleteIndex = BirthdayUsers.FindIndex()
        // if (BirthdayUsers.Select(user => user.Id).Contains(userId)) {

        // }
        return true;
    }
}