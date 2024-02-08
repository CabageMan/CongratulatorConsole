using System.Runtime.Serialization;
using Datasource;

namespace Models;

public class CongratulatorModel
{
    private readonly IDatasource? datasource;

    public List<BirthdayPerson> BirthdayPersons { get; set; }

    public CongratulatorModel(Action<string> callback)
    {
        try
        {
            // datasource = new FileDatasource();
            datasource = new MySQLDatasource();
            BirthdayPersons = ConvertFromRawBirthdays(datasource.GetAllBirthdays());
        }
        catch (InvalidOperationException e) 
        {
            BirthdayPersons = [];
            datasource = null;
            callback(e.Message);
        }
    }

    public void AddNewBirthday(
        PersonRole role,
        string firstName,
        string lastName,
        DateOnly birthDate)
    {
        int lastPersonId = BirthdayPersons.Count == 0 ? 0 : BirthdayPersons.Last().Id;
        BirthdayPerson newPerson = new(++lastPersonId, firstName, lastName, birthDate, role);
        BirthdayPersons.Add(newPerson);

        if (datasource != null)
        {
            datasource.AddNewBirthday(new RawBirthday(
                newPerson.Id,
                newPerson.RoleString,
                newPerson.FirstName,
                newPerson.LastName,
                newPerson.BirthDate));
        }
        else
        {
            throw new InvalidOperationException($"Record{newPerson.Id}: Could not store persistently - datasource is not available.");
        }
    }

    public void DeleteBirthdayBy(int birthdayId)
    {
        BirthdayPersons.RemoveAll(person => person.Id == birthdayId);

        if (datasource != null)
        {
            datasource.DeleteBirthdayBy(birthdayId);
        }
        else
        {
            throw new InvalidOperationException($"Record{birthdayId}: Could not delete from storage - datasource is not available.");
        }
    }

    public void EditBirthday(BirthdayPerson editedBirthdayPerson)
    {
        int editedPersonIndex = BirthdayPersons
            .FindIndex(person => person.Id == editedBirthdayPerson.Id);

        if (BirthdayPersons.Count > editedPersonIndex)
        {
            BirthdayPersons[editedPersonIndex] = editedBirthdayPerson;
        }
        else
        {
            throw new InvalidOperationException($"Record{editedBirthdayPerson.Id}: Could not find a record to edit");
        }

        if (datasource != null)
        {
            datasource.ReplaceBirthday(new RawBirthday(
                editedBirthdayPerson.Id,
                editedBirthdayPerson.RoleString,
                editedBirthdayPerson.FirstName,
                editedBirthdayPerson.LastName,
                editedBirthdayPerson.BirthDate
            ));
        }
        else 
        {
            throw new InvalidOperationException($"Record{editedBirthdayPerson.Id}: Could store edited record - datasource is not available.");
        }
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
                throw new InvalidOperationException($"Record{rawBirthday.Id}: Could not parse role or date");
            }
        }
        return birthdayPersons;
    }
}