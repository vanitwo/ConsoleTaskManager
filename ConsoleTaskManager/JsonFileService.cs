using System.Text.Json;

namespace ConsoleTaskManager;

public class JsonFileService
{
    private readonly string _filePath;

    public JsonFileService(string filePath = "tasks.json")
    {
        _filePath = filePath;
    }

    public List<TaskItem> LoadTasks()
    {
        try
        {
            if (!File.Exists(_filePath)) return new List<TaskItem>();

            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<TaskItem>>(json)
                   ?? new List<TaskItem>();
        }
        catch (Exception ex)
        {
            PrintMessage.ShowError($"Ошибка загрузки: {ex.Message}");
            return new List<TaskItem>();
        }
    }

    public void SaveTasks(List<TaskItem> tasks)
    {
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(tasks, options);
            File.WriteAllText(_filePath, json);
        }
        catch (Exception ex)
        {
            PrintMessage.ShowError($"Ошибка сохранения: {ex.Message}");
        }
    }
}
