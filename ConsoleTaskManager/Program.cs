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

    public static void PrintColoredCenteredLine(string text, ConsoleColor color, int verticalOffset = 0)
    {
        // Сохраняем исходные настройки
        var originalColor = Console.ForegroundColor;
        int originalX = Console.CursorLeft;
        int originalY = Console.CursorTop;

        // Рассчитываем позицию
        int x = (Console.WindowWidth - text.Length) / 2;
        int y = (Console.WindowHeight / 2) + verticalOffset; // Центр экрана + смещение

        // Устанавливаем позицию и цвет
        Console.SetCursorPosition(x, y);
        Console.ForegroundColor = color;
        Console.Write(text);

        // Восстанавливаем настройки
        Console.ForegroundColor = originalColor;
        Console.SetCursorPosition(originalX, originalY); // Возвращаем курсор на исходную позицию
    }

    public static void PrintColoredCenteredBlock(List<Tuple<string, ConsoleColor>> lines)
    {
        int startY = -(lines.Count / 2); // Центрируем блок целиком

        foreach (var line in lines)
        {
            PrintColoredCenteredLine(line.Item1, line.Item2, startY);
            startY++;
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
    private static int _idCounter = 1;
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
        Id = _idCounter;
        _idCounter++;
    }

    public ConsoleColor GetTaskColor()
    {
        if (IsCompleted) return ConsoleColor.Green;
        if (DueDate < DateTime.Now) return ConsoleColor.Red;
        return ConsoleColor.Blue;
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
            PrintMessage.ShowError("Список задач пуст!");
            return;
        }

        // Собираем все строки с их цветами
        var linesWithColors = new List<Tuple<string, ConsoleColor>>();
        linesWithColors.Add(Tuple.Create("=== СПИСОК ЗАДАЧ ===", ConsoleColor.White));

        foreach (var task in TaskItems)
        {
            ConsoleColor taskColor = task.GetTaskColor();
            linesWithColors.Add(Tuple.Create($"ID: {task.Id}", taskColor));
            linesWithColors.Add(Tuple.Create($"Название: {task.Title}", taskColor));
            linesWithColors.Add(Tuple.Create($"Цель: {task.Description}", taskColor));
            linesWithColors.Add(Tuple.Create($"Срок: {task.DueDate:dd.MM.yyyy HH:mm}", taskColor));
            linesWithColors.Add(Tuple.Create(new string('-', 30), taskColor));
        }

        PrintMessage.PrintColoredCenteredBlock(linesWithColors);

        Console.ReadKey();
        Console.Clear();
    }

    public void MarkTaskDone()
    {
        ShowTasks();

        PrintMessage.PrintCenteredText("Введите ID задачи для отметки:");
        string input = Console.ReadLine();

        var taskId = CheckTaskId(input);
        var task = TaskItems.FirstOrDefault(task => task.Id == taskId);

        task.IsCompleted = true;

        Console.Clear();
        PrintMessage.PrintColoredCenteredLine($"Задача '{task.Title}' выполнена!", ConsoleColor.Green);
        Console.ReadKey();
        Console.Clear();
    }

    public void DeleteTask()
    {
        ShowTasks();

        PrintMessage.PrintCenteredText("Введите ID задачи для удаления:");
        string input = Console.ReadLine();
        var taskId = CheckTaskId(input);
        var task = TaskItems.FirstOrDefault(task => task.Id == taskId);
        TaskItems.Remove(task);

        Console.Clear();
        PrintMessage.PrintColoredCenteredLine($"Задача '{task.Title}' удалена!", ConsoleColor.Yellow);
        Console.ReadKey();
        Console.Clear();
    }

    private int CheckTaskId(string input)
    {
        if (!int.TryParse(input, out int taskId))
            PrintMessage.ShowError("Некорректный ID!");

        var task = TaskItems.FirstOrDefault(task => task.Id == taskId);

        if (task == null)
            PrintMessage.ShowError("Задача с таким ID не найдена!");

        return taskId;
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