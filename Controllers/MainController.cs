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
            AddNewUser);
        congratulatorModel = new CongratulatorModel();
    }

    public void Start() 
    {
        // Mock data
        congratulatorModel.AddNewUser(UserRole.FamilarPerson, "Blob", "Jr", DateOnly.FromDateTime(DateTime.Now));
        congratulatorModel.AddNewUser(UserRole.Friend, "Blob", "Sr", DateOnly.FromDateTime(DateTime.Now));
        congratulatorModel.AddNewUser(UserRole.FamilarPerson, "Greg", "Jhonson", DateOnly.FromDateTime(DateTime.Now));

        menuController.Start();
    }

    private void ShowAllBirthdays() 
    {
        MenuController.ShowBirthdays(congratulatorModel.BirthdayUsers);
    }

    private void ShowUpcommingBitrhdays() 
    {
        // Make two lists to show today birthdays at first, and then upcomming birthdays (up to 3 days).
        var currentDate = DateOnly.FromDateTime(DateTime.Now);
        var todayBirthdays = congratulatorModel.BirthdayUsers.Where(user => 
            user.BirthDate.Month.Equals(currentDate.Month) && user.BirthDate.Day.Equals(currentDate.Day)
        ).ToList();
        var upcommingBirthdays = congratulatorModel.BirthdayUsers.Where(user => {
            var lowBound = currentDate.DayNumber - user.BirthDate.DayNumber > 0; 
            var upperBound = currentDate.DayNumber - user.BirthDate.DayNumber <= 3; 
            Console.WriteLine($"Lower: {currentDate.DayNumber}\nUpper: {user.BirthDate.DayNumber}");
            Console.ReadKey();
            return lowBound && upperBound;
        }).ToList();
        todayBirthdays.AddRange(upcommingBirthdays);
        MenuController.ShowBirthdays(todayBirthdays);
    }

    private void AddNewUser(string firstName, string lastName, UserRole userRole, DateOnly birthDate) 
    {
        // Check if date is not future, only real facts
        congratulatorModel.AddNewUser(userRole, firstName, lastName, birthDate);
    }
}