using System.Xml;
using System.Xml.Linq;
using Datasource;
using Microsoft.VisualBasic;
using Models;

namespace Datasource;

public class FileDatasource : IDatasource
{
    private const string FILE_PATH = "Resources/Birthdays.xml";
    private const string BIRTHDAY_RECORD_NODE_NAME = "BirthdayRecord";
    private const string ID_NODE_NAME = "Id";
    private const string ROLE_NODE_NAME = "Role";
    private const string FIRSTNAME_NODE_NAME = "FirstName";
    private const string LASTNAME_NODE_NAME = "LastName";
    private const string BIRTHDATE_NODE_NAME = "BirthDate";

    private readonly XDocument document;

    public FileDatasource()
    {
        try
        {
            document = XDocument.Load(FILE_PATH);
        }
        catch (DirectoryNotFoundException e)
        {
            throw new InvalidOperationException(e.Message);
        }
    }

    List<BirthdayPerson> IDatasource.GetAllBirthdays()
    {
        IEnumerable<BirthdayPerson> birstdays = [];
        if (document.Root != null)
        {
            birstdays = document.Root
                .Descendants(BIRTHDAY_RECORD_NODE_NAME)
                .Select(record => ParseBirthdayRecord(record));
        }
        else
        {
            throw new InvalidOperationException("Root Element is null");
        }

        return birstdays.ToList();
    }

    long IDatasource.AddNewBirthday(BirthdayPerson birthday)
    {
        if (document.Root != null)
        {
            var recordToAdd = ConvertToXmlBirthdayRecord(birthday);
            document.Root.Add(recordToAdd);
            document.Save(FILE_PATH);
            return birthday.Id;
        }
        else
        {
            throw new InvalidOperationException("Root Element is null");
        }
    }

    void IDatasource.DeleteBirthdayBy(long birthdayId)
    {
        if (document.Root != null)
        {
            var birthdayToDelete = document.Root
                .Descendants(BIRTHDAY_RECORD_NODE_NAME)
                .First(record =>
                {
                    var idNode = record.Element(ID_NODE_NAME);
                    return idNode != null ? int.Parse(idNode.Value) == birthdayId : false;
                });
            birthdayToDelete.Remove();
            document.Save(FILE_PATH);
        }
        else
        {
            throw new InvalidOperationException("Root Element is null");
        }
    }

    void IDatasource.ReplaceBirthday(BirthdayPerson birthday)
    {
        if (document.Root != null)
        {
            var birthdayToDelete = document.Root
                .Descendants(BIRTHDAY_RECORD_NODE_NAME)
                .First(record =>
                {
                    var idNode = record.Element(ID_NODE_NAME);
                    return idNode != null ? int.Parse(idNode.Value) == birthday.Id : false;
                });
            birthdayToDelete.ReplaceWith(ConvertToXmlBirthdayRecord(birthday));
            document.Save(FILE_PATH);
        }
        else
        {
            throw new InvalidOperationException("Root Element is null");
        }
    }

    // TODO: Refactor parsing in async way
    private static BirthdayPerson ParseBirthdayRecord(XElement record)
    {
        long id;
        var idNode = record.Element(ID_NODE_NAME);
        if (idNode != null)
        {
            id = long.Parse(idNode.Value);
        }
        else
        {
            throw new InvalidOperationException($"{ID_NODE_NAME} node is null");
        }

        PersonRole role = PersonRole.FamilarPerson; // Default value
        var roleNode = record.Element(ROLE_NODE_NAME);
        if (roleNode == null && !Enum.TryParse(roleNode?.Value, true, out role))
        {
            throw new InvalidOperationException($"{ROLE_NODE_NAME} node is null");
        }

        string firstName;
        var firstNameNode = record.Element(FIRSTNAME_NODE_NAME);
        if (firstNameNode != null)
        {
            firstName = firstNameNode.Value;
        }
        else
        {
            throw new InvalidOperationException($"{FIRSTNAME_NODE_NAME} node is null");
        }

        string lastName;
        var lastNameNode = record.Element(LASTNAME_NODE_NAME);
        if (lastNameNode != null)
        {
            lastName = lastNameNode.Value;
        }
        else
        {
            throw new InvalidOperationException($"{LASTNAME_NODE_NAME} node is null");
        }

        var birthDateNode = record.Element(BIRTHDATE_NODE_NAME);
        if (birthDateNode != null && DateOnly.TryParse(birthDateNode.Value, out DateOnly birthDate))
        {
            return new BirthdayPerson(
                id,
                firstName,
                lastName,
                birthDate,
                role
            );
        }
        else
        {
            throw new InvalidOperationException($"{BIRTHDATE_NODE_NAME} node is null");
        }
    }

    private static XElement ConvertToXmlBirthdayRecord(BirthdayPerson birthday)
    {
        return new XElement(BIRTHDAY_RECORD_NODE_NAME,
            new XElement(ID_NODE_NAME, birthday.Id),
            new XElement(ROLE_NODE_NAME, birthday.RoleString),
            new XElement(FIRSTNAME_NODE_NAME, birthday.FirstName),
            new XElement(LASTNAME_NODE_NAME, birthday.LastName),
            new XElement(BIRTHDATE_NODE_NAME, birthday.BirthDateString)
        );
    }
}
