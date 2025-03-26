namespace ConsoleTaskManager;

public class Program
{
    private static bool _isRunning = true;
    private static TaskManager _taskManager = new TaskManager();

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
            case "1": _taskManager.AddTask(); break;
            case "2": _taskManager.ShowTasks(); break;
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
    private int _idCounter = 1;
    public int Id { get; } 
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
        Id = _idCounter++;
    }
}

public class TaskManager
{
    public List<TaskItem> TaskItems { get; set; } = new List<TaskItem>();

    public void AddTask()
    {       
        string title = GetInput("Введите название задачи: ", "Описание задачи не может быть пустым!");
        if (title == null) return;
        Console.Clear();
        
        string description = GetInput("Введите цель задачи: ", "Цель задачи не может быть пустой!");
        if (description == null) return;
        Console.Clear();
        
        DateTime? dueDate = GetDueDate();
        if (!dueDate.HasValue) return;
        Console.Clear();
        
        TaskItems.Add(new TaskItem(title, description, dueDate.Value));
        PrintMessage.PrintCenteredText("Задача добавлена!");
    }

    public void ShowTasks()
    {
        if (!TaskItems.Any())
        {
            PrintMessage.PrintCenteredText("Список задач пуст!");
            return;
        }

        var output = new List<string> { "СПИСОК ЗАДАЧ", " "};
        

        foreach (var task in TaskItems)
        {
            var taskLines = new[]
            {
                $"ID: {task.Id}",
                $"Название: {task.Title}",
                $"Описание: {task.Description}",
                $"Срок: {task.DueDate:dd.MM.yyyy HH:mm}"
            };

            output.AddRange(taskLines);
            output.Add(new string('-', taskLines.Max(line => line.Length)));
        }
        //output = output
        //    //.Skip(1)
        //    .Select(line => line.PadRight(35)).ToList();

        PrintMessage.PrintCenteredText(output.ToArray());
        Console.ReadKey();
    }

    private string GetInput(string prompt, string errorMessage)
    {
        PrintMessage.PrintCenteredText(prompt);
        string input = Console.ReadLine()?.Trim();

        if (string.IsNullOrEmpty(input))
        {
            PrintMessage.ShowError(errorMessage);
            return null;
        }
        return input;
    }

    private DateTime? GetDueDate()
    {
        PrintMessage.PrintCenteredText("Сколько дней на выполнение задачи: ");
        string input = Console.ReadLine()?.Trim();

        if (!int.TryParse(input, out int days) || days <= 0)
        {
            PrintMessage.ShowError("Некорректный ввод! Введите положительное число.");
            return null;
        }

        return DateTime.Now.AddDays(days);
    }
}