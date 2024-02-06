using Microsoft.VisualBasic;
using Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;

namespace Controllers;

public class MenuController(
    Action showBirthdays,
    Action showUpcommingBirthdays,
    Action<string, string, PersonRole, DateOnly> addNewBirthday,
    Action showBirthdaysToDelete,
    Action<int> deleteBirthday,
    Action showBirthdaysToEdit,
    Action<BirthdayPerson> editBirthday)
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

    public enum BirthdaysActionMenuItem
    {
        ShowAll,
        ShowUpcomming,
        Edit,
        Delete
    }

    private bool exitApp = false;

    private readonly Action _showBirthdays = showBirthdays;
    private readonly Action _showUpcommingBirthdays = showUpcommingBirthdays;
    private readonly Action<string, string, PersonRole, DateOnly> _addNewBirthday = addNewBirthday;
    private readonly Action _showBirthdaysToDelete = showBirthdaysToDelete;
    private readonly Action<int> _deleteBirthday = deleteBirthday;
    private readonly Action _showBirthdaysToEdit = showBirthdaysToEdit;
    private readonly Action<BirthdayPerson> _editBirthday = editBirthday;

    private List<string> _warnings = [];

    public List<string> Warnings
    {
        get => _warnings;
        set => _warnings = value;
    }

    public void Start()
    {
        Console.Clear();
        while (!exitApp)
        {
            ShowMainMenu();
        }
    }

    // Show menus 
    private void ShowMainMenu()
    {
        ShowWarnigsIfExist(_warnings);
        ShowMenuTitle("Enter a number of menu item:");
        ShowMenuItems<MainMenuItem>();
        HandleSelectedMainMenuItem(GetUsersMenuItemInput<MainMenuItem>());
    }

    // TODO: Give an oportunity to leave this menu, like in deleting or editing
    // TODO: Also make possible to skip items and left previous value
    private void ShowAddBirthdayMenu()
    {
        Console.Clear();
        ShowWarnigsIfExist(_warnings);
        ShowMenuTitle("Select person role:");
        ShowMenuItems<PersonRole>();
        HandleAddingNewPerson(GetUsersMenuItemInput<PersonRole>());
    }

    public void ShowBirthdaysListWithAction(
        List<BirthdayPerson> birthdayPersons,
        BirthdaysActionMenuItem action)
    {
        switch (action)
        {
            case BirthdaysActionMenuItem.ShowAll:
                ShowWarnigsIfExist(_warnings);
                ShowMenuTitle("All birthdays list:");
                ShowBirthdaysList(birthdayPersons);
                GetOnlyExitUserInput();
                break;
            case BirthdaysActionMenuItem.ShowUpcomming:
                ShowWarnigsIfExist(_warnings);
                ShowMenuTitle("Today and upcomming birthdays:");
                ShowBirthdaysList(birthdayPersons);
                GetOnlyExitUserInput();
                break;
            case BirthdaysActionMenuItem.Delete:
                ShowWarnigsIfExist(_warnings);
                ShowMenuTitle("Enter birthday ID to delete: ");
                ShowBirthdaysList(birthdayPersons);
                GetDeleteBirthdayUserInput(birthdayPersons.Select(person => person.Id));
                break;
            case BirthdaysActionMenuItem.Edit:
                ShowWarnigsIfExist(_warnings);
                ShowMenuTitle("Enter birthday ID to edit: ");
                ShowBirthdaysList(birthdayPersons);
                GetEditBirthdayUserInput(birthdayPersons);
                break;
        }
    }

    // Draw content in console
    private static void ShowWarnigsIfExist(List<string> warnings)
    {
        if (warnings.Count != 0)
        {
            Console.ForegroundColor = WARNING_COLOR;
            Console.WriteLine("We have some promlems: ");
            foreach (string warningMessage in warnings)
            {
                Console.WriteLine("  -" + warningMessage + ";");
            }
            Console.Write(Environment.NewLine);
        }
    }
    private static void ShowMenuTitle(string title)
    {
        Console.ForegroundColor = TITLE_COLOR;
        Console.WriteLine(title + Environment.NewLine, Console.ForegroundColor);
    }

    private static void ShowMenuItems<E>() where E : Enum
    {
        Console.ForegroundColor = ITEM_COLOR;
        foreach (int enumItem in Enum.GetValues(typeof(E)))
        {
            Console.Write($"\t{enumItem} - ");
            Console.WriteLine($"{Enum.GetName(typeof(E), enumItem)}");
        }
        Console.Write(Environment.NewLine);
    }

    private static void ShowBirthdaysList(List<BirthdayPerson> birthdayPersons)
    {
        if (birthdayPersons.Count != 0)
        {
            DrawBirthdaysTable(birthdayPersons);
        }
        else
        {
            Console.ForegroundColor = WARNING_COLOR;
            Console.WriteLine("There are no birthdays records yet");
            Console.ForegroundColor = ITEM_COLOR;
        }
    }

    private static void DrawBirthdaysTable(List<BirthdayPerson> birthdayPersons)
    {
        string spacer = " | ";
        var spaceLength = spacer.Length;
        var idColumnLength = birthdayPersons
            .Select(person => person.Id.ToString().Length)
            .Max() + spaceLength;
        var roleColumnLength = birthdayPersons
            .Select(person => person.RoleString.Length)
            .Max() + spaceLength;
        var nameColumnLength = birthdayPersons
            .Select(person => person.FullName.Length)
            .Max() + spaceLength;
        var birthDateColumnLength = birthdayPersons
            .Select(person => person.BirthDateString.Length)
            .Max() + spaceLength;

        var titlesWithSpaces = new (string title, int space)[] {
            ("ID", idColumnLength),
            ("Role", roleColumnLength),
            ("Name", nameColumnLength),
            ("Birth Date", birthDateColumnLength)
        };
        DrawTableHeader(titlesWithSpaces, spacer);

        foreach (BirthdayPerson person in birthdayPersons)
        {
            Console.WriteLine(
                person.Id + WhiteSpaces(idColumnLength - person.Id.ToString().Length) + spacer +
                person.Role + WhiteSpaces(roleColumnLength - person.RoleString.Length) + spacer +
                person.FullName + WhiteSpaces(nameColumnLength - person.FullName.Length) + spacer +
                person.BirthDateString + WhiteSpaces(birthDateColumnLength - person.BirthDateString.Length) + spacer
            );
        }

        Console.Write(Environment.NewLine);
    }

    private static void DrawTableHeader((string, int)[] titlesWithSpaces, string spacer)
    {
        Console.ForegroundColor = WARNING_COLOR;
        foreach ((string title, int space) in titlesWithSpaces)
        {
            var whiteSpace = WhiteSpaces(space - title.Length);
            Console.Write(title + whiteSpace + spacer);
        }
        Console.Write(Environment.NewLine);
        Console.ForegroundColor = ITEM_COLOR;
    }

    private static string WhiteSpaces(int count)
    {
        return new string(' ', count);
    }

    // Get user input
    private static E GetUsersMenuItemInput<E>() where E : Enum
    {
        int menuItemsCount = Enum.GetValues(typeof(E)).Length;
        Console.Write("> ");
        var userInput = Console.ReadLine();
        var menuItemNumber = menuItemsCount;

        var allCases = (IEnumerable<int>)Enum.GetValues(typeof(E));
        var minValue = allCases.ToArray().Min();

        while (!int.TryParse(userInput, out menuItemNumber) || !Enum.IsDefined(typeof(E), menuItemNumber))
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
        } while (userInput != "x");
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
            }
            else if (int.TryParse(userInput, out int selectedId) && birthdaysIdsList.Contains(selectedId))
            {
                while (true)
                {
                    Console.Write("Are you shure? yes/no\n> ");
                    var confirmInput = Console.ReadLine();
                    if (confirmInput == "y" || confirmInput == "yes")
                    {
                        _deleteBirthday(selectedId);
                        break;
                    }
                    else if (confirmInput == "n" || confirmInput == "no")
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

    private void GetEditBirthdayUserInput(List<BirthdayPerson> birthdayPersons)
    {
        Console.ForegroundColor = TITLE_COLOR;
        while (true)
        {
            Console.Write("Enter birthday Id you want to edit or\ntype \"x\" to return to the main menu\n> ");
            var userInput = Console.ReadLine();
            var birthdaysIdsList = birthdayPersons.Select(person => person.Id);
            if (userInput == "x")
            {
                break;
            }
            else if (int.TryParse(userInput, out int selectedId) && birthdaysIdsList.Contains(selectedId))
            {
                HandleEditingBirthday(birthdayPersons, selectedId);
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
        var currentDate = DateOnly.FromDateTime(DateTime.Now);
        var inputBirthDate = currentDate;
        while (
            !DateOnly.TryParse(inputBirthDateString, out inputBirthDate) ||
            inputBirthDate > currentDate)
        {
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
                ShowAddBirthdayMenu();
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

    private void HandleAddingNewPerson(PersonRole personRole)
    {
        Console.Clear();
        Console.WriteLine($"Enter new {personRole} first name:{Environment.NewLine}");
        Console.Write("> ");
        var personFirstName = Console.ReadLine();

        Console.WriteLine($"Enter {personFirstName}'s last name:{Environment.NewLine}");
        Console.Write("> ");
        var personLastName = Console.ReadLine();

        Console.WriteLine($"Enter {personFirstName}'s {personLastName} bith date in format MM/dd/yyyy (not future dates):{Environment.NewLine}");
        Console.Write("> ");
        var personBirthDate = GetUserDateInput();

        _addNewBirthday(
            personFirstName ?? "",
            personLastName ?? "",
            personRole,
            personBirthDate);

        Console.Clear();
    }

    private void HandleEditingBirthday(List<BirthdayPerson> birthdayPersons, int selectedId)
    {
        var selectedPerson = birthdayPersons.Find(person => person.Id == selectedId);
        Console.Clear();
        ShowMenuTitle($"Current Role is {selectedPerson.Role} enter new one:");
        ShowMenuItems<PersonRole>();
        var updatedRole = GetUsersMenuItemInput<PersonRole>();

        Console.Clear();
        Console.WriteLine($"Current First Name is {selectedPerson.FirstName} enter new one:{Environment.NewLine}");
        Console.Write("> ");
        var updatedFirstName = Console.ReadLine() ?? "";

        Console.WriteLine($"Current Last Name is {selectedPerson.LastName} ented new one:{Environment.NewLine}");
        Console.Write("> ");
        var updatedLastName = Console.ReadLine() ?? "";

        Console.WriteLine($"Current Birth Date is {selectedPerson.BirthDateString} enter new one in format MM/dd/yyyy:{Environment.NewLine}");
        Console.Write("> ");
        var updatedBirthDate = GetUserDateInput();

        _editBirthday(new BirthdayPerson(
            selectedId,
            updatedFirstName,
            updatedLastName,
            updatedBirthDate,
            updatedRole)
            );

        Console.Clear();
    }
}