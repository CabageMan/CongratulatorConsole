using Models;

namespace Controllers;

public class MainController
{
    private MenuController menuController;
    private CongratulatorModel congratulatorModel;

    public MainController() 
    {
        menuController = new MenuController(ShowAllBirthdays, AddNewUser);
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
        MenuController.ShowAllBirthdays(congratulatorModel.BirthdayUsers);
    }

    private void AddNewUser(string firstName, string lastName, UserRole userRole, DateOnly birthDate) 
    {
        // Check if date is not future, only real facts
        congratulatorModel.AddNewUser(userRole, firstName, lastName, birthDate);
    }
}