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


    private readonly XmlDocument document;

    public FileDatasource()
    {
        document = new();
        document.Load(FILE_PATH);
    }

    List<RawBirthday> IDatasource.GetAllBirthdays()
    {
        var xml = XDocument.Load(FILE_PATH);

        IEnumerable<RawBirthday> birstdays = [];
        if (xml.Root != null)
        {
            birstdays = xml.Root
                .Descendants(BIRTHDAY_RECORD_NODE_NAME)
                .Select(record => ParseBirthdayRecord(record));
        }
        else
        {
            throw new InvalidOperationException("Root Element is null");
        }

        return birstdays.ToList();
    }

    void IDatasource.PutAllBirthdays(List<RawBirthday> rawBirthdays)
    {
        // TODO: Handle all exceptions on writing.
        // document.RemoveAll();
        // document.WriteContentTo(xmlTextWriter);
    }

    void IDatasource.AddNewBirthday(RawBirthday rawBirthday)
    {
        var xml = XDocument.Load(FILE_PATH);
        var recordToAdd = ConvertToXmlBirthdayRecord(rawBirthday);
        Console.WriteLine("Record to Add" + recordToAdd.ToString());
        xml.Root.Add(recordToAdd);
        xml.Save(FILE_PATH);
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
