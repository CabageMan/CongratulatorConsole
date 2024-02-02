using System.Xml;
using System.Xml.Linq;
using Datasource;
using Microsoft.VisualBasic;
using Models;

namespace Datasource;

public class FileDatasource : IDatasource
{
    // TODO: Move file path to config file and environment variable
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
        document = XDocument.Load(FILE_PATH);
    }

    List<RawBirthday> IDatasource.GetAllBirthdays()
    {
        IEnumerable<RawBirthday> birstdays = [];
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

    void IDatasource.AddNewBirthday(RawBirthday rawBirthday)
    {
        if (document.Root != null)
        {
            var recordToAdd = ConvertToXmlBirthdayRecord(rawBirthday);
            document.Root.Add(recordToAdd);
            document.Save(FILE_PATH);
        }
        else
        {
            throw new InvalidOperationException("Root Element is null");
        }
    }

    void IDatasource.DeleteBirthdayBy(int birthdayId)
    {
        if (document.Root != null)
        {
            var birthdayToDelete = document.Root
                .Descendants(BIRTHDAY_RECORD_NODE_NAME)
                .First(record => {
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

    void IDatasource.ReplaceBirthday(Datasource.RawBirthday rawBirthday)
    {
        if (document.Root != null)
        {
            var birthdayToDelete = document.Root
                .Descendants(BIRTHDAY_RECORD_NODE_NAME)
                .First(record => {
                    var idNode = record.Element(ID_NODE_NAME);
                    return idNode != null ? int.Parse(idNode.Value) == rawBirthday.Id : false;
                });
            birthdayToDelete.ReplaceWith(ConvertToXmlBirthdayRecord(rawBirthday));
            document.Save(FILE_PATH);
        }
        else 
        {
            throw new InvalidOperationException("Root Element is null");
        }
    }

    // TODO: Refactor parsing in async way
    private static RawBirthday ParseBirthdayRecord(XElement record)
    {
        int id;
        var idNode = record.Element(ID_NODE_NAME);
        if (idNode != null)
        {
            id = int.Parse(idNode.Value);
        }
        else
        {
            throw new InvalidOperationException($"{ID_NODE_NAME} node is null");
        }

        string roleStrirng;
        var roleNode = record.Element(ROLE_NODE_NAME);
        if (roleNode != null)
        {
            roleStrirng = roleNode.Value;
        }
        else
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

        string birthDateStrirng;
        var birthDateNode = record.Element(BIRTHDATE_NODE_NAME);
        if (birthDateNode != null)
        {
            birthDateStrirng = birthDateNode.Value;
        }
        else
        {
            throw new InvalidOperationException($"{BIRTHDATE_NODE_NAME} node is null");
        }

        return new RawBirthday(
            id,
            roleStrirng,
            firstName,
            lastName,
            birthDateStrirng
        );
    }

    private static XElement ConvertToXmlBirthdayRecord(RawBirthday rawBirthday)
    {
        return new XElement(BIRTHDAY_RECORD_NODE_NAME,
            new XElement(ID_NODE_NAME, rawBirthday.Id),
            new XElement(ROLE_NODE_NAME, rawBirthday.RoleString),
            new XElement(FIRSTNAME_NODE_NAME, rawBirthday.FirstName),
            new XElement(LASTNAME_NODE_NAME, rawBirthday.LastName),
            new XElement(BIRTHDATE_NODE_NAME, rawBirthday.BirthDateString)
        );
    }
}
