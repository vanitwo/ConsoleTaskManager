namespace ConsoleTaskManager
{
    public class TaskManager
    {
        private readonly JsonFileService _fileService;
        public List<TaskItem> TaskItems { get; set; }

        public TaskManager(JsonFileService fileService)
        {
            _fileService = fileService;
            TaskItems = _fileService.LoadTasks();
            UpdateIdCounter();
        }

        private void PersistData()
        {
            _fileService.SaveTasks(TaskItems);
            UpdateIdCounter();
        }

        private void UpdateIdCounter()
        {
            if (TaskItems.Any())
            {
                TaskItem._idCounter = (TaskItems.Max(t => t.Id) + 1);
            }
        }

        public void AddTask()
        {
            string title = GetInput("Введите название задачи: ", "Описание задачи не может быть пустым!");
            if (title == null) return;
            Console.Clear();

            string description = GetInput("Введите цель задачи: ", "Цель задачи не может быть пустой!");
            if (description == null) return;
            Console.Clear();

            var dueDate = GetDueDate();
            if (!dueDate.HasValue) return;
            Console.Clear();

            TaskItems.Add(new TaskItem(title, description, dueDate.Value));
            PersistData();
            PrintMessage.PrintCenteredText("Задача добавлена!");
        }

        public void ShowTasks()
        {
            if (!TaskItems.Any())
            {
                PrintMessage.ShowError("Список задач пуст!");
                return;
            }

            var sortedTasks = TaskItems
            .OrderBy(t => t.IsCompleted)
            .ThenBy(t => t.DueDate)
            .ToList();

            // Собираем все строки с их цветами
            var linesWithColors = new List<Tuple<string, ConsoleColor>>();
            linesWithColors.Add(Tuple.Create("=== СПИСОК ЗАДАЧ ===", ConsoleColor.White));

            foreach (var task in sortedTasks)
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
            var taskId = CheckTaskId(Console.ReadLine());
            if (taskId == null) return;

            var task = TaskItems.First(t => t.Id == taskId);
            PrintMessage.PrintCenteredText($"Пометить выполненой задачу '{task.Title}'? (Y/N)");
            var confirm = Console.ReadLine()?.Trim().ToUpper();
            if (confirm != "Y") return;

            task.IsCompleted = true;
            PersistData();

            Console.Clear();
            PrintMessage.PrintColoredCenteredLine($"Задача '{task.Title}' выполнена!", ConsoleColor.Green);
            Console.ReadKey();
            Console.Clear();
        }

        public void DeleteTask()
        {
            ShowTasks();

            PrintMessage.PrintCenteredText("Введите ID задачи для удаления:");
            var taskId = CheckTaskId(Console.ReadLine());
            if (taskId == null) return;

            var task = TaskItems.First(t => t.Id == taskId);
            PrintMessage.PrintCenteredText($"Удалить задачу '{task.Title}'? (Y/N)");
            var confirm = Console.ReadLine()?.Trim().ToUpper();
            if (confirm != "Y") return;

            TaskItems.Remove(task);
            PersistData();

            Console.Clear();
            PrintMessage.PrintColoredCenteredLine($"Задача '{task.Title}' удалена!", ConsoleColor.Yellow);
            Console.ReadKey();
            Console.Clear();
        }

        private int? CheckTaskId(string input)
        {
            if (!int.TryParse(input, out int taskId))
            {
                PrintMessage.ShowError("Некорректный ID!");
                return null;
            }

            var task = TaskItems.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
            {
                PrintMessage.ShowError("Задача с таким ID не найдена!");
                return null;
            }

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
}
