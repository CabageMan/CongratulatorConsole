using Datasource;

namespace Models;

public class CongratulatorModel
{
    private readonly IDatasource? datasource;

    public List<BirthdayPerson> BirthdayPersons { get; set; }

    public CongratulatorModel(DatasourceType datasourceType, Action<string> callback)
    {
        try
        {
            switch (datasourceType)
            {
                case DatasourceType.FileDatasource:
                    datasource = new FileDatasource();
                    break;
                case DatasourceType.DataBase:
                    datasource = new MySQLDatasource();
                    break;
                default:
                    datasource = new FileDatasource();
                    break;
            }
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
        if (datasource != null)
        {
            try
            {
                datasource.DeleteBirthdayBy(birthdayId);
                BirthdayPersons.RemoveAll(person => person.Id == birthdayId);
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException($"Could not delete record with ID={birthdayId}: " + e.Message);
            }
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

        if (BirthdayPersons.Count > editedPersonIndex && datasource != null)
        {
            try
            {
                datasource.ReplaceBirthday(editedBirthdayPerson);
                BirthdayPersons[editedPersonIndex] = editedBirthdayPerson;
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException($"Could store edited record: " + e.Message);
            }
        }
        else
        {
            throw new InvalidOperationException($"Could not edit record with ID={editedBirthdayPerson.Id}");
        }

    }
}