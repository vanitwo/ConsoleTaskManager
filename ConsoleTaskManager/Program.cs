using System.Text.Json;

namespace ConsoleTaskManager;

public class Program
{
    private static bool _isRunning = true;
    private static TaskManager _taskManager;

    public static void Main()
    {
        Console.CursorVisible = false;
        Console.SetWindowSize(100, 50);
        Console.SetBufferSize(100, 50);

        var fileService = new JsonFileService();
        var inputProvider = new ConsoleInputProvider();
        _taskManager = new TaskManager(fileService, inputProvider);

        PrintGreetings();
        while (_isRunning)
        {
            PrintMenu();
            TaskSelection();
        }
    }

    private static void TaskSelection()
    {
        var userInput = Console.ReadLine()?.Trim() ?? string.Empty;
        Console.Clear();

        switch (userInput)
        {
            case "1": _taskManager.AddTask(); break;
            case "2": _taskManager.ShowTasks(); break;
            case "3": _taskManager.MarkTaskDone(); break;
            case "4": _taskManager.DeleteTask(); break;
            case "5": ExitApp(); break;
            default: PrintMessage.ShowError("Неверный номер операции! Попробуй еще раз."); break;
        }
    }

    private static void PrintMenu()
    {
        int menuWidth = 40;

        string[] menuItems =
        {
            "МЕНЮ:",
            "1. Добавить задачу",
            "2. Посмотреть все задачи",
            "3. Отметить задачу как выполненную",
            "4. Удалить задачу",
            "5. Выход",
            "Введите номер операции:"
        };

        menuItems = menuItems
                    .Select(line => line.PadRight(35))
                    .ToArray();

        PrintMessage.PrintCenteredText(menuItems);
    }

    private static void PrintGreetings()
    {
        string[] greetings =
        {
            "Добро пожаловать в Консольный список задач!",
            "Чтобы начать, нажмите любую клавишу"
        };

        PrintMessage.PrintCenteredText(greetings);
        Console.ReadKey();
        Console.Clear();
    }    

    private static void ExitApp()
    {
        PrintMessage.PrintCenteredText("Вы уверены, что хотите выйти? (Y/N)");
        if (Console.ReadKey().Key == ConsoleKey.Y)        
            _taskManager.PersistData();   
        
        Console.Clear();
        PrintMessage.PrintCenteredText(new[] { "Выход из приложения...", " ", "До свидания!" });        
        Thread.Sleep(1500);
        _isRunning = false;
    }
}