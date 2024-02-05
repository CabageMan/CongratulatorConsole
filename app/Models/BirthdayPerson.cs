namespace Models;

public enum PersonRole
{
    Friend = 1,
    FamilarPerson,
    Colleague,
    Employee
}

public struct BirthdayPerson(
    int id,
    string firstName,
    string lastName,
    DateOnly birthDate,
    PersonRole role)
{
    private readonly int _id = id;
    private DateOnly _birthDate = birthDate;
    private String _firstName = firstName;
    private String _lastName = lastName;
    private PersonRole _role = role;

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
    public PersonRole Role
    {
        get => _role;
        set => _role = value;
    }
    public readonly string RoleString
    {
        get => _role.ToString();
    }

    public readonly override string ToString()
    {
        return $"ID: {_id}; Role: {RoleString}; Name: {FullName}; Birth Date: {BirthDateString}";
    }
}
