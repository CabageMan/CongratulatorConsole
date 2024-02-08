using Models;

namespace Datasource;

public struct RawBirthday(
    int id,
    string role,
    string firstName,
    string lastName,
    DateOnly birthDate)
{
    private readonly int _id = id;
    private string _role = role;
    private string _firstName = firstName;
    private string _lastName = lastName;
    private DateOnly _birthDate = birthDate;

    public readonly int Id 
    {
        get => _id;
    }
    public DateOnly BirthDate 
    { 
        get => _birthDate; 
        set => _birthDate = value; 
    }
    public readonly string BirthDateString 
    { 
        get => $"{_birthDate:MM-dd-yyyy}";
    }
    public string FirstName 
    { 
        get => _firstName; 
        set => _firstName = value; 
    }
    public string LastName 
    { 
        get => _lastName; 
        set => _lastName = value; 
    }
    public string RoleString 
    {
        get => _role;
        set => _role = value;
    }

    public override string ToString()
    {
        return $"ID: {_id}; Role: {RoleString}; Name: {FirstName} {LastName}; Birth Date: {BirthDateString}";
    }
}


public interface IDatasource
{
    List<RawBirthday> GetAllBirthdays();
    void AddNewBirthday(RawBirthday rawBirthday);
    void DeleteBirthdayBy(int birthdayId);
    void ReplaceBirthday(RawBirthday rawBirthday);
}

