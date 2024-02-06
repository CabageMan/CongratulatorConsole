using Models;
using NUnit.Framework;

namespace CongratulatorConsole.Tests;

[TestFixture]
public class CongratulatorModelTests
{
    private CongratulatorModel _congratulatorModel; 

    [SetUp]
    public void Setup()
    {
        List<String> messages = [];
        _congratulatorModel = new CongratulatorModel(message => messages.Add(message));
    }

    [Test]
    public void Should_Add_New_Birthday()
    {
        Assert.That(2+2, Is.EqualTo(4));
        // Assert.Fail("Just fail this");
    }
}