namespace ConsoleTaskManager;

public class Program
{
    private static bool _isRunning = true;

    public static void Main()
    {
        Console.CursorVisible = false;

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
            case "1": ExecuteAction("Добавляю задачу"); break;
            case "2": ExecuteAction("Сейчас покажу все задачи"); break;
            case "3": ExecuteAction("Отмечаю задачу выполненной"); break;
            case "4": ExecuteAction("Удаляю задачу"); break;
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


    private static void ExecuteAction(string message)
    {
        PrintMessage.PrintCenteredText(new[] { message, " ", "Нажмите любую клавишу для продолжения..." });
        Console.ReadKey();
        Console.Clear();
    }

    private static void ExitApp()
    {
        PrintMessage.PrintCenteredText(new[] { "Выход из приложения...", " ", "До свидания!" });
        Thread.Sleep(1500);
        _isRunning = false;
    }
}

public class PrintMessage
{
    public static void PrintCenteredText(params string[] lines)
    {
        int verticalStart = (Console.WindowHeight - lines.Length) / 2;
        verticalStart = Math.Max(verticalStart, 0);

        foreach (var line in lines)
        {
            int horizontalStart = (Console.WindowWidth - line.Length) / 2;
            horizontalStart = Math.Max(horizontalStart, 0);

            Console.SetCursorPosition(horizontalStart, verticalStart++);
            Console.Write(line);
        }
    }

    public static void ShowError(string error)
    {

        Console.ForegroundColor = ConsoleColor.Red;
        PrintCenteredText(new[] { error, " ", "Нажмите любую клавишу для продолжения..." });
        Console.ResetColor();
        Console.ReadKey();
        Console.Clear();
    }
}

public class TaskItem
{
    public int Id { get; } = 0;
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; }

    public TaskItem(string title, string description, DateTime dueDate)
    {
        Title = title;
        Description = description;
        DueDate = dueDate;
        IsCompleted = false;
        Id++;
    }
}

public class TaskManager
{
    public List<TaskItem> TaskItems { get; set; } = new List<TaskItem>();

    public void AddTask()
    {
        PrintMessage.PrintCenteredText("Введите название задачи: ");
        var title = Console.ReadLine()?.Trim();
        if (!string.IsNullOrEmpty(title))
        {
            PrintMessage.PrintCenteredText("Задача добавлена!");
        }
        else
        {
            PrintMessage.ShowError("Описание задачи не может быть пустым!");
        }

        PrintMessage.PrintCenteredText("Введите цель задачи: ");
        var description = Console.ReadLine()?.Trim();
        if (!string.IsNullOrEmpty(description))
        {
            PrintMessage.PrintCenteredText("Цель утановлена!");
        }
        else
        {
            PrintMessage.ShowError("Цель задачи не может быть пустой!");
        }

        PrintMessage.PrintCenteredText("Сколько времени вы хотите потратить на задачу в днях: ");
        var dueTime = DateTime.Now;
        if (int.TryParse(Console.ReadLine()?.Trim(), out int days))
        {
            dueTime.AddDays(days);
            PrintMessage.PrintCenteredText($"Дата {days} дней назад: {dueTime}");
        }
        else
        {
            PrintMessage.ShowError("Некорректный ввод");
        }
        
        TaskItems.Add(new TaskItem(title, description, dueTime));
    }
}