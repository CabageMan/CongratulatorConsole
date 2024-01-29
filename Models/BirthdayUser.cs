namespace Models;

public enum UserRole 
{
    Friend = 1,
    FamilarPerson,
    Colleague,
    Employee
}

public struct BirthdayUser
{
    private readonly int _id;
    private DateOnly _birthDate;
    private String _firstName;
    private String _lastName;
    private UserRole _role;

    public readonly int Id 
    {
        get => _id;
    }
    public DateOnly BirthDate 
    { 
        get => _birthDate; 
        set => _birthDate = value; 
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

    public BirthdayUser(int id, string firstName, string lastName, DateOnly birthDate, UserRole role)
    {
        _id = id;
        _role = role;
        _birthDate = birthDate;
        _firstName = firstName;
        _lastName = lastName;
    }

    public override string ToString()
    {
        return $"ID: {_id}; Role: {Role}; Name: {FullName}; Birth Date: {_birthDate:MM-dd-yyyy}";
    }
}
