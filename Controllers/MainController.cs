using Models;

namespace Controllers;

public class MainController
{
    private MenuController menuController;
    private CongratulatorModel congratulatorModel;

    public MainController() 
    {
        menuController = new MenuController();
        congratulatorModel = new CongratulatorModel();
    }

    public void Start() 
    {
        menuController.Start();
        // test
        var date = DateOnly.FromDateTime(DateTime.Now);
        congratulatorModel.AddNewUser(UserRole.Friend, "Blob", "Junior", date);
        foreach(BirthdayUser user in congratulatorModel.BirthdayUsers) {
            Console.WriteLine($"First person is: {user.ToString()}");
        }
    }
}