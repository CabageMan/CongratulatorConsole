namespace Models;

public enum UserRole 
{
    Friend = 1,
    FamilarPerson,
    Colleague,
    Employee
}

public struct BirthdayUser(
    int id, 
    string firstName, 
    string lastName, 
    DateOnly birthDate, 
    UserRole role)
{
    private readonly int _id = id;
    private DateOnly _birthDate = birthDate;
    private String _firstName = firstName;
    private String _lastName = lastName;
    private UserRole _role = role;

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
    public readonly String FullName 
    { 
        get => $"{_firstName} {_lastName}"; 
    }
    public UserRole Role 
    {
        get => _role;
        set => _role = value;
    }

    public override string ToString()
    {
        return $"ID: {_id}; Role: {Role}; Name: {FullName}; Birth Date: {BirthDateString}";
    }
}
