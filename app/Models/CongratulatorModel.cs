using System.Runtime.Serialization;
using Datasource;
using Org.BouncyCastle.Asn1.Misc;

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
            BirthdayPersons = datasource.GetAllBirthdays();
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
        long lastPersonId = BirthdayPersons.Count == 0 ? 0 : BirthdayPersons.Last().Id;
        if (datasource != null)
        {
            try
            {
                // Calculating and setting new personId id required for FileDatasourse
                // MySqlDatasourse returns last added to database ID
                BirthdayPerson newPerson = new(++lastPersonId, firstName, lastName, birthDate, role);
                var newId = datasource.AddNewBirthday(newPerson);
                newPerson.Id = newId;

                BirthdayPersons.Add(newPerson);
            }
            catch (InvalidOperationException e) 
            {
                throw new InvalidOperationException("Could not add record: " + e.Message);
            }
        }
        else
        {
            throw new InvalidOperationException($"Record{lastPersonId}: Could not store persistently - datasource is not available.");
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
            datasource.ReplaceBirthday(editedBirthdayPerson);
        }
        else
        {
            throw new InvalidOperationException($"Record{editedBirthdayPerson.Id}: Could store edited record - datasource is not available.");
        }
    }
}