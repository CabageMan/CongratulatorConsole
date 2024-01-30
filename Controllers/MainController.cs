using Models;

namespace Controllers;

public class MainController
{
    private MenuController menuController;
    private CongratulatorModel congratulatorModel;

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
        // Mock data
        congratulatorModel.AddNewBirthday(UserRole.FamilarPerson, "Blob", "Jr", DateOnly.FromDateTime(DateTime.Now));
        congratulatorModel.AddNewBirthday(UserRole.Friend, "Blob", "Sr", DateOnly.FromDateTime(DateTime.Now));
        congratulatorModel.AddNewBirthday(UserRole.FamilarPerson, "Greg", "Jhonson", DateOnly.FromDateTime(DateTime.Now));

        menuController.Start();
    }

    private void ShowAllBirthdays() 
    {
        MenuController.ShowBirthdays(congratulatorModel.BirthdayUsers, "All birthdays list:");
    }

    private void ShowUpcommingBitrhdays() 
    {
        // Make two lists to show today birthdays at first, and then upcomming birthdays (up to 3 days).
        var currentDate = DateOnly.FromDateTime(DateTime.Now);
        var todayBirthdays = congratulatorModel.BirthdayUsers.Where(user => 
            user.BirthDate.Month.Equals(currentDate.Month) && user.BirthDate.Day.Equals(currentDate.Day)
        ).ToList();
        var upcommingBirthdays = congratulatorModel.BirthdayUsers.Where(user => {
            var leftCondition = user.BirthDate.DayOfYear - currentDate.DayOfYear > 0; 
            var rightCondition = user.BirthDate.DayOfYear - currentDate.DayOfYear <= 3; 
            return leftCondition && rightCondition;
        }).ToList();
        todayBirthdays.AddRange(upcommingBirthdays);
        MenuController.ShowBirthdays(todayBirthdays, "Today and upcomming birthdays:");
    }

    private void AddNewBirthday(string firstName, string lastName, UserRole userRole, DateOnly birthDate) 
    {
        // ToDo: Check if date is not future, only real facts
        congratulatorModel.AddNewBirthday(userRole, firstName, lastName, birthDate);
    }

    private void ShowBirthdaysToDelete() 
    {
        menuController.ShowDeleteBirthdayMenu(congratulatorModel.BirthdayUsers);
    }

    private void ShowBirthdaysToEdit() 
    {
        menuController.ShowEditBirthdayMenu(congratulatorModel.BirthdayUsers);
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