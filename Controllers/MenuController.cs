using Microsoft.VisualBasic;
using Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;

namespace Controllers;

public class MenuController(
    Action showBirthdays,
    Action showUpcommingBirthdays,
    Action<string, string, UserRole, DateOnly> addNewBirthday,
    Action showBirthdaysToDelete,
    Action<int> deleteBirthday,
    Action showBirthdaysToEdit,
    Action<BirthdayUser> editBirthday)
{
    private const ConsoleColor TITLE_COLOR = ConsoleColor.Green;
    private const ConsoleColor ITEM_COLOR = ConsoleColor.Blue;
    private const ConsoleColor ERROR_COLOR = ConsoleColor.Red;
    private const ConsoleColor WARNING_COLOR = ConsoleColor.Magenta;

    enum MainMenuItem 
    {
        ShowAll = 1,
        ShowUpcoming,
        AddNew,
        Delete,
        Edit, 
        Exit
    }

    public enum BirthdaysListMenuItem
    {
        ShowAll,
        ShowUpcomming,
        Edit,
        Delete
    }

    private bool exitApp = false;

    private readonly Action _showBirthdays = showBirthdays;
    private readonly Action _showUpcommingBirthdays = showUpcommingBirthdays;
    private readonly Action<string, string, UserRole, DateOnly> _addNewBirthday = addNewBirthday;
    private readonly Action _showBirthdaysToDelete = showBirthdaysToDelete;
    private readonly Action<int> _deleteBirthday = deleteBirthday;
    private readonly Action _showBirthdaysToEdit = showBirthdaysToEdit;
    private readonly Action<BirthdayUser> _editBirthday = editBirthday;

    public void Start() 
    {
        Console.Clear();
        while(!exitApp)
        {
            ShowMainMenu();
        }
    }

    // Show menus 
    private void ShowMainMenu() 
    {
        ShowMenuTitle("Enter a number of menu item:");
        ShowMenuItems<MainMenuItem>();
        HandleSelectedMainMenuItem(GetUsersMenuItemInput<MainMenuItem>());
    }

    private void ShowAddUserMenu() 
    {
        Console.Clear();
        ShowMenuTitle("Select user role:");
        ShowMenuItems<UserRole>();
        HandleAddingNewUser(GetUsersMenuItemInput<UserRole>());
    }

    public void ShowBirthdaysListWithAction(
        List<BirthdayUser> birthdayUsers, 
        BirthdaysListMenuItem action)
    {
        switch (action)
        {
            case BirthdaysListMenuItem.ShowAll:
                ShowMenuTitle("All birthdays list:");
                ShowBirthdaysList(birthdayUsers);
                GetOnlyExitUserInput();
                break;
            case BirthdaysListMenuItem.ShowUpcomming:
                ShowMenuTitle("Today and upcomming birthdays:");
                ShowBirthdaysList(birthdayUsers);
                GetOnlyExitUserInput();
                break;
            case BirthdaysListMenuItem.Delete:
                ShowMenuTitle("Enter birthday ID to delete: ");
                ShowBirthdaysList(birthdayUsers);       
                GetDeleteBirthdayUserInput(birthdayUsers.Select(user => user.Id));
                break;
            case BirthdaysListMenuItem.Edit:
                ShowMenuTitle("Enter birthday ID to edit: ");
                ShowBirthdaysList(birthdayUsers);       
                GetEditBirthdayUserInput(birthdayUsers);
                break;
        }
    }

    // Draw content in console
    private static void ShowMenuTitle(string title)
    {
        Console.ForegroundColor = TITLE_COLOR;
        Console.WriteLine(title + Environment.NewLine, Console.ForegroundColor);
    }

    private static void ShowMenuItems<E>() where E: Enum 
    {
        Console.ForegroundColor = ITEM_COLOR;
        foreach(int enumItem in Enum.GetValues(typeof(E))) 
        {
            Console.Write($"\t{enumItem} - ");
            Console.WriteLine($"{Enum.GetName(typeof(E), enumItem)}");
        }
        Console.Write(Environment.NewLine);
    }

    private static void ShowBirthdaysList(List<BirthdayUser> birthdayUsers) 
    {
        if (birthdayUsers.Count != 0) {
            DrawBirthdaysTable(birthdayUsers);
        } else {
            Console.ForegroundColor = WARNING_COLOR;
            Console.WriteLine("There are no birthdays records yet", Console.ForegroundColor);
            Console.ForegroundColor = ITEM_COLOR;
        }
    }

    private static void DrawBirthdaysTable(List<BirthdayUser> birthdayUsers) 
    {
        string spacer = " | ";
        var spaceLength = spacer.Length;
        var idColumnLength = birthdayUsers.Select(user => user.Id.ToString().Length).Max() + spaceLength;
        var roleColumnLength = birthdayUsers.Select(user => user.Role.ToString().Length).Max() + spaceLength;
        var nameColumnLength = birthdayUsers.Select(user => user.FullName.Length).Max() + spaceLength;
        var birthDateColumnLength = birthdayUsers.Select(user => user.BirthDateString.Length).Max() + spaceLength;

        var titlesWithSpaces = new(string title, int space) [] { 
            ("ID", idColumnLength), 
            ("Role", roleColumnLength), 
            ("Name", nameColumnLength), 
            ("Birth Date", birthDateColumnLength) 
        };
        DrawTableHeader(titlesWithSpaces, spacer);

        foreach(BirthdayUser user in birthdayUsers) {
            Console.WriteLine(
                $"{user.Id}{WhiteSpaces(idColumnLength - user.Id.ToString().Length)}{spacer}" + 
                $"{user.Role}{WhiteSpaces(roleColumnLength - user.Role.ToString().Length)}{spacer}" +
                $"{user.FullName}{WhiteSpaces(nameColumnLength - user.FullName.Length)}{spacer}" +
                $"{user.BirthDateString}{WhiteSpaces(birthDateColumnLength - user.BirthDateString.Length)}{spacer}"
            );
        }

        Console.Write(Environment.NewLine);
    }

    private static void DrawTableHeader((string, int)[] titlesWithSpaces, string spacer) 
    {
        Console.ForegroundColor = WARNING_COLOR;
        foreach((string title, int space) in titlesWithSpaces)
        {
            var whiteSpace = WhiteSpaces(space - title.Length);
            Console.Write($"{title}{whiteSpace}{spacer}");
        }
        Console.Write(Environment.NewLine);
        Console.ForegroundColor = ITEM_COLOR;
    }

    private static string WhiteSpaces(int count)
    {
        return new string(' ', count);
    }

    // Get user input
    private static E GetUsersMenuItemInput<E>() where E: Enum
    {
        int menuItemsCount = Enum.GetValues(typeof(E)).Length;
        Console.Write("> ");
        var userInput = Console.ReadLine();
        var menuItemNumber = menuItemsCount;

        var allCases = (IEnumerable<int>)Enum.GetValues(typeof(E));
        var minValue = allCases.ToArray().Min();

        while(!int.TryParse(userInput, out menuItemNumber) || !Enum.IsDefined(typeof(E), menuItemNumber))
        {
            Console.ForegroundColor = ERROR_COLOR;
            Console.WriteLine($"There is no menu item with number {menuItemNumber}.{Environment.NewLine}Please enter number between {minValue} and {menuItemsCount}", Console.ForegroundColor);            
            Console.ForegroundColor = ITEM_COLOR;
            Console.Write("> ");
            userInput = Console.ReadLine();
        }

        return (E)Enum.ToObject(typeof(E), menuItemNumber);
    }

    private static void GetOnlyExitUserInput() 
    {
        var userInput = "";
        Console.ForegroundColor = TITLE_COLOR;
        do 
        {

            Console.Write("Type \"x\" to return to the main menu\n> ");
            userInput = Console.ReadLine();
        } while(userInput != "x");
        Console.Clear();
    }

    private void GetDeleteBirthdayUserInput(IEnumerable<int> birthdaysIdsList) 
    {
        Console.ForegroundColor = TITLE_COLOR;
        while (true) 
        {
            Console.Write("Enter birthday Id to delete or\ntype \"x\" to return to the main menu\n> ");
            var userInput = Console.ReadLine();
            if (userInput == "x")
            {
                break;
            } else if (int.TryParse(userInput, out int selectedId) && birthdaysIdsList.Contains(selectedId))
            {
                while (true) 
                {
                    Console.Write("Are you shure? yes/no\n> ");
                    var confirmInput = Console.ReadLine();
                    if (confirmInput == "y" || confirmInput == "yes")
                    {
                        _deleteBirthday(selectedId);
                        break;
                    } else if (confirmInput == "n" || confirmInput == "no")
                    {
                        break;
                    }
                }
                break;
            } 
            else 
            {
                continue;
            }
        }
        Console.Clear();
    }

    private void GetEditBirthdayUserInput(List<BirthdayUser> birthdayUsers)
    {
        Console.ForegroundColor = TITLE_COLOR;
        while(true) 
        {
            Console.Write("Enter birthday Id you want to edit or\ntype \"x\" to return to the main menu\n> ");
            var userInput = Console.ReadLine();
            var birthdaysIdsList = birthdayUsers.Select(user => user.Id);
            if (userInput == "x")
            {
                break;
            } else if (int.TryParse(userInput, out int selectedId) && birthdaysIdsList.Contains(selectedId))
            {
                HandleEditingBirthday(birthdayUsers, selectedId);
                break;
            } 
            else 
            {
                continue;
            }
        }
        Console.Clear();
    }

    private static DateOnly GetUserDateInput() 
    {
        var inputBirthDateString = Console.ReadLine();
        var inputBirthDate = DateOnly.FromDateTime(DateTime.Now);
        while(!DateOnly.TryParse(inputBirthDateString, out inputBirthDate)) {
            Console.ForegroundColor = ERROR_COLOR;
            Console.WriteLine($"Date input is incorrect {inputBirthDateString}.{Environment.NewLine}Please enter correct date in format MM/dd/yyyy", Console.ForegroundColor);            
            Console.ForegroundColor = ITEM_COLOR;
            Console.Write("> ");
            inputBirthDateString = Console.ReadLine();
        }
        return inputBirthDate;
    }

    // Handle User Input
    private void HandleSelectedMainMenuItem(MainMenuItem item) 
    {
        Console.Clear();

        switch (item) 
        {
            case MainMenuItem.ShowAll:
                _showBirthdays();
                break;
            case MainMenuItem.ShowUpcoming:
                _showUpcommingBirthdays();
                break;
            case MainMenuItem.AddNew:
                ShowAddUserMenu();
                break;
            case MainMenuItem.Delete:
                _showBirthdaysToDelete();
                break;
            case MainMenuItem.Edit:
                _showBirthdaysToEdit();
                break;
            case MainMenuItem.Exit:
                Console.WriteLine("Exiting the application...");
                exitApp = true;
                break;
        }
    }

    private void HandleAddingNewUser(UserRole userRole) {
        Console.Clear();
        Console.WriteLine($"Enter new {userRole} first name:{Environment.NewLine}");
        Console.Write("> ");
        var userFirstName = Console.ReadLine();

        Console.WriteLine($"Enter {userFirstName}'s last name:{Environment.NewLine}");
        Console.Write("> ");
        var userLastName = Console.ReadLine();

        Console.WriteLine($"Enter {userFirstName}'s {userLastName} bith date in format MM/dd/yyyy:{Environment.NewLine}");
        Console.Write("> ");
        var userBirthDate = GetUserDateInput();

        _addNewBirthday(userFirstName ?? "", userLastName ?? "", userRole, userBirthDate);

        Console.Clear();
    }

    private void HandleEditingBirthday(List<BirthdayUser> birthdayUsers, int selectedId)
    {
        var selectedUser = birthdayUsers.Find(user => user.Id == selectedId);
        Console.Clear();
        ShowMenuTitle($"Current Role is {selectedUser.Role} enter new one:");
        ShowMenuItems<UserRole>();
        var updateRole = GetUsersMenuItemInput<UserRole>();

        Console.Clear();
        Console.WriteLine($"Current First Name is {selectedUser.FirstName} enter new one:{Environment.NewLine}");
        Console.Write("> ");
        var updatedFirstName = Console.ReadLine() ?? "";

        Console.WriteLine($"Current Last Name is {selectedUser.LastName} ented new one:{Environment.NewLine}");
        Console.Write("> ");
        var updatedLastName = Console.ReadLine() ?? "";

        Console.WriteLine($"Current Birth Date is {selectedUser.BirthDateString} enter new one in format MM/dd/yyyy:{Environment.NewLine}");
        Console.Write("> ");
        var updatedBirthDate = GetUserDateInput();

        _editBirthday(new BirthdayUser(
            selectedId,
            updatedFirstName,
            updatedLastName, 
            updatedBirthDate, 
            updateRole)
            );

        Console.Clear();
    }
}