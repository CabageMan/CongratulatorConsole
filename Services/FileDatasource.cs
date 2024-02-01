

using System.Xml;
using Datasource;
using Microsoft.VisualBasic;

namespace Datasource;

public class FileDatasource: IDatasource
{
    // TODO: Move file path to config file and environment variable
    private const string FILE_PATH = "Resources/Birthdays.xml";
    private XmlDocument document;
    public FileDatasource() 
    {
        document = new();
        document.Load(FILE_PATH);
    }

    List<RawBirthday> IDatasource.GetAllBirthdayUsers() {
        return ParseXmlBirthdays(document.DocumentElement);
    }

    // TODO: Refactor parsing
    private static List<RawBirthday> ParseXmlBirthdays(XmlElement? root) 
    {
        List<RawBirthday> rawBirthdays = [];

        if (root != null)
        {
            foreach (XmlNode node in root.ChildNodes) 
            {
                int id;
                var idNode = node["Id"];
                if (idNode != null) {
                    id = int.Parse(idNode.InnerText);
                } else {
                    throw new ArgumentNullException("Id");
                }

                string roleStrirng;
                var roleNode = node["Role"];
                if (roleNode != null) {
                    roleStrirng = roleNode.InnerText;
                } else {
                    throw new ArgumentNullException("Role");
                }

                string firstName;
                var firstNameNode = node["FirstName"];
                if (firstNameNode != null) {
                    firstName = firstNameNode.InnerText;
                } else {
                    throw new ArgumentNullException("FirstName");
                }

                string lastName;
                var lastNameNode = node["LastName"];
                if (lastNameNode != null) {
                    lastName = lastNameNode.InnerText;
                } else {
                    throw new ArgumentNullException("LastName");
                }

                string birthDateStrirng;
                var birthDateNode = node["BirthDate"];
                if (birthDateNode != null) {
                    birthDateStrirng = birthDateNode.InnerText;
                } else {
                    throw new ArgumentNullException("BirthDate");
                }

                rawBirthdays.Add(new(
                    id, 
                    roleStrirng, 
                    firstName, 
                    lastName, 
                    birthDateStrirng)
                );
            }

            return rawBirthdays;
        } 
        else 
        {
            throw new ArgumentNullException("Xml Document");
        }
    }
}
