using System.Runtime.Serialization;
using Datasource;

namespace Models;

public class CongratulatorModel
{
    private readonly IDatasource datasource;
    public List<BirthdayPerson> BirthdayPersons { get; set; }

    public CongratulatorModel()
    {
        datasource = new FileDatasource();
        BirthdayPersons = ConvertFromRawBirthdays(datasource.GetAllBirthdays());
    }

    // Could throw an error if validation or adding is failed
    public bool AddNewBirthday(PersonRole role, string firstName, string lastName, DateOnly birthDate)
    {
        // Add validations if names empty, role is wrong and birthday is in future in controller
        int lastPersonId = BirthdayPersons.Count == 0 ? 0 : BirthdayPersons.Last().Id;
        BirthdayPerson newPerson = new(++lastPersonId, firstName, lastName, birthDate, role);
        BirthdayPersons.Add(newPerson);

        datasource.AddNewBirthday(new RawBirthday(
            newPerson.Id,
            newPerson.RoleString,
            newPerson.FirstName,
            newPerson.LastName,
            newPerson.BirthDateString));

        return true;
    }

    // Could throw an error if deletion is failed
    public bool DeleteBirthdayBy(int birthdayId)
    {
        // Handle exceptions
        BirthdayPersons.RemoveAll(person => person.Id == birthdayId);
        datasource.DeleteBirthdayBy(birthdayId);
        return true;
    }

    public bool EditBirthday(BirthdayPerson editedBirthdayPerson)
    {
        // Handle exceptions
        int editedPersonIndex = BirthdayPersons.FindIndex(person => person.Id == editedBirthdayPerson.Id);
        BirthdayPersons[editedPersonIndex] = editedBirthdayPerson;
        datasource.ReplaceBirthday(new RawBirthday(
            editedBirthdayPerson.Id,
            editedBirthdayPerson.RoleString,
            editedBirthdayPerson.FirstName,
            editedBirthdayPerson.LastName,
            editedBirthdayPerson.BirthDateString
        ));

        return true;
    }

    private static List<BirthdayPerson> ConvertFromRawBirthdays(List<RawBirthday> rawBirthdays)
    {
        List<BirthdayPerson> birthdayPersons = [];
        foreach (RawBirthday rawBirthday in rawBirthdays)
        {
            if (
                Enum.TryParse(rawBirthday.RoleString, true, out PersonRole role) &&
                DateOnly.TryParse(rawBirthday.BirthDateString, out DateOnly birthDate))
            {
                birthdayPersons.Add(new(
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
        return birthdayPersons;
    }
}