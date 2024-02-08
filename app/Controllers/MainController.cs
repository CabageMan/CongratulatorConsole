using Datasource;
using Models;

namespace Controllers;

public class MainController
{
    private readonly MenuController menuController;
    private readonly CongratulatorModel congratulatorModel;

    public MainController()
    {
        DatasourceType selectedDatasource = DatasourceType.FileDatasource; // Default value
        menuController = new MenuController(
            datasource => selectedDatasource = datasource,
            ShowAllBirthdays,
            ShowUpcommingBitrhdays,
            AddNewBirthday,
            ShowBirthdaysToDelete,
            DeleteBirthday,
            ShowBirthdaysToEdit,
            EditBirthday);
        menuController.SelectDatasource();

        congratulatorModel = new CongratulatorModel(selectedDatasource, message => menuController.Warnings.Add(message));
    }

    public void Start()
    {
        menuController.Start();
    }

    private void ShowAllBirthdays()
    {
        menuController.ShowBirthdaysListWithAction(
            congratulatorModel.BirthdayPersons,
            MenuController.BirthdaysActionMenuItem.ShowAll);
    }

    private void ShowUpcommingBitrhdays()
    {
        // Make two lists to show today birthdays at first, and then upcomming birthdays (up to 3 days).
        var currentDate = DateOnly.FromDateTime(DateTime.Now);
        var allBirthdays = congratulatorModel.BirthdayPersons
            .Where(user => user.BirthDate.Month.Equals(currentDate.Month) && user.BirthDate.Day.Equals(currentDate.Day))
            .ToList();
        var upcommingBirthdays = congratulatorModel.BirthdayPersons
            .Where(user =>
            {
                var leftCondition = user.BirthDate.DayOfYear - currentDate.DayOfYear > 0;
                var rightCondition = user.BirthDate.DayOfYear - currentDate.DayOfYear <= 3;
                return leftCondition && rightCondition;
            })
            .ToList();
        allBirthdays.AddRange(upcommingBirthdays);
        menuController.ShowBirthdaysListWithAction(
            allBirthdays,
            MenuController.BirthdaysActionMenuItem.ShowUpcomming);
    }

    private void AddNewBirthday(
        string firstName,
        string lastName,
        PersonRole personRole,
        DateOnly birthDate)
    {
        try
        {
            congratulatorModel.AddNewBirthday(personRole, firstName, lastName, birthDate);
        }
        catch (InvalidOperationException e)
        {
            menuController.Warnings.Add(e.Message);
        }
    }

    private void ShowBirthdaysToDelete()
    {
        menuController.ShowBirthdaysListWithAction(
            congratulatorModel.BirthdayPersons,
            MenuController.BirthdaysActionMenuItem.Delete);
    }

    private void ShowBirthdaysToEdit()
    {
        menuController.ShowBirthdaysListWithAction(
            congratulatorModel.BirthdayPersons,
            MenuController.BirthdaysActionMenuItem.Edit);
    }

    private void DeleteBirthday(int Id)
    {
        try
        {
            congratulatorModel.DeleteBirthdayBy(Id);
        }
        catch (InvalidOperationException e)
        {
            menuController.Warnings.Add(e.Message);
        }
    }

    private void EditBirthday(BirthdayPerson editedBirthdayPerson)
    {
        try
        {
            congratulatorModel.EditBirthday(editedBirthdayPerson);
        }
        catch (InvalidOperationException e)
        {
            menuController.Warnings.Add(e.Message);
        }
    }
}