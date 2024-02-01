using Models;

namespace Controllers;

public class MainController
{
    private readonly MenuController menuController;
    private readonly CongratulatorModel congratulatorModel;

    public MainController() 
    {
        menuController = new MenuController(
            ShowAllBirthdays,
            ShowUpcommingBitrhdays,
            AddNewBirthday,
            ShowBirthdaysToDelete,
            DeleteBirthday,
            ShowBirthdaysToEdit,
            EditBirthday);
        congratulatorModel = new CongratulatorModel();
    }

    public void Start() 
    {
        menuController.Start();
    }

    private void ShowAllBirthdays() 
    {
        menuController.ShowBirthdaysListWithAction(
            congratulatorModel.BirthdayUsers, 
            MenuController.BirthdaysListMenuItem.ShowAll);
    }

    private void ShowUpcommingBitrhdays() 
    {
        // Make two lists to show today birthdays at first, and then upcomming birthdays (up to 3 days).
        var currentDate = DateOnly.FromDateTime(DateTime.Now);
        var allBirthdays = congratulatorModel.BirthdayUsers.Where(user => 
            user.BirthDate.Month.Equals(currentDate.Month) && user.BirthDate.Day.Equals(currentDate.Day)
        ).ToList();
        var upcommingBirthdays = congratulatorModel.BirthdayUsers.Where(user => {
            var leftCondition = user.BirthDate.DayOfYear - currentDate.DayOfYear > 0; 
            var rightCondition = user.BirthDate.DayOfYear - currentDate.DayOfYear <= 3; 
            return leftCondition && rightCondition;
        }).ToList();
        allBirthdays.AddRange(upcommingBirthdays);
        menuController.ShowBirthdaysListWithAction(
            allBirthdays, 
            MenuController.BirthdaysListMenuItem.ShowUpcomming);
    }

    private void AddNewBirthday(string firstName, string lastName, UserRole userRole, DateOnly birthDate) 
    {
        // ToDo: Check if date is not future, only real facts
        congratulatorModel.AddNewBirthday(userRole, firstName, lastName, birthDate);
    }

    private void ShowBirthdaysToDelete() 
    {
        menuController.ShowBirthdaysListWithAction(
            congratulatorModel.BirthdayUsers,
            MenuController.BirthdaysListMenuItem.Delete);
    }

    private void ShowBirthdaysToEdit() 
    {
        menuController.ShowBirthdaysListWithAction(
            congratulatorModel.BirthdayUsers,
            MenuController.BirthdaysListMenuItem.Edit);
    }

    private void DeleteBirthday(int Id) 
    {
        congratulatorModel.DeleteBirthdayBy(Id);
    }

    private void EditBirthday(BirthdayUser editedBirthdayUser) 
    {
        congratulatorModel.EditBirthday(editedBirthdayUser);
    }
}