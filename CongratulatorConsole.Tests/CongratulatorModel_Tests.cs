using Datasource;
using Models;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace CongratulatorConsole.Tests;

[TestFixture]
public class CongratulatorModelTests
{
    private CongratulatorModel _congratulatorModel;

    [SetUp]
    public void Setup()
    {
        _congratulatorModel = new CongratulatorModel(DatasourceType.FileDatasource, message => { });
    }
    // TODO:
    // Need to use test environment for tests (test Xml file and test database)!

    [Test]
    public void ShouldAddAndDeleteBirthdayToFileDatasource()
    {
        _= DateOnly.TryParse("01.01.1001", out DateOnly testDate);
        long lastPersonId = _congratulatorModel.BirthdayPersons.Count == 0 ? 0 : _congratulatorModel.BirthdayPersons.Last().Id;
        long testID = lastPersonId + 1;
        BirthdayPerson testPerson = new(
            testID,
            "Test",
            "Tester",
            testDate,
            PersonRole.Friend);

        _congratulatorModel.AddNewBirthday(
            PersonRole.Friend,
            "Test",
            "Tester",
            testDate
        );

        Assert.That(_congratulatorModel.BirthdayPersons.Contains(testPerson), Is.EqualTo(true));

        _congratulatorModel.DeleteBirthdayBy(testID);

        Assert.That(_congratulatorModel.BirthdayPersons.Contains(testPerson), Is.EqualTo(false));
    }

    [Test]
    public void ShouldEditBirthdayInFileDatasource()
    {
        _= DateOnly.TryParse("01.01.1001", out DateOnly testDate);
        long lastPersonId = _congratulatorModel.BirthdayPersons.Count == 0 ? 0 : _congratulatorModel.BirthdayPersons.Last().Id;
        long testID = lastPersonId + 1;
        BirthdayPerson testPerson = new(
            testID,
            "Test",
            "Tester",
            testDate,
            PersonRole.Friend);

        BirthdayPerson testEditedPerson = new(
            testID,
            "Edited",
            "Editor",
            testDate,
            PersonRole.Friend);

        _congratulatorModel.AddNewBirthday(
            PersonRole.Friend,
            "Test",
            "Tester",
            testDate
        );
        Assert.That(_congratulatorModel.BirthdayPersons.Contains(testPerson), Is.EqualTo(true));

        _congratulatorModel.EditBirthday(testEditedPerson);
        Assert.Multiple(() =>
        {
            Assert.That(_congratulatorModel.BirthdayPersons.Contains(testEditedPerson), Is.EqualTo(true));
            Assert.That(_congratulatorModel.BirthdayPersons.Contains(testPerson), Is.EqualTo(false));
        });

        _congratulatorModel.DeleteBirthdayBy(testID);
        Assert.Multiple(() =>
        {
            Assert.That(_congratulatorModel.BirthdayPersons.Contains(testPerson), Is.EqualTo(false));
            Assert.That(_congratulatorModel.BirthdayPersons.Contains(testEditedPerson), Is.EqualTo(false));
        });
    }
}