using ConsoleTaskManager;
using Moq;

namespace TaskManagers.Tests;

public class JsonFileServiceTests
{
    [Fact]
    public void LoadTasks_WhenFileNotExists_ReturnsEmptyList()
    {
        // Arrange
        var service = new JsonFileService("non_existent.json");

        // Act
        var result = service.LoadTasks();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void SaveAndLoad_ShouldPreserveData()
    {
        // Arrange
        var tempFile = Path.GetTempFileName();
        var service = new JsonFileService(tempFile);
        var tasks = new List<TaskItem>
        {
            new TaskItem { Id = 1, Title = "Test" }
        };

        // Act
        service.SaveTasks(tasks);
        var loaded = service.LoadTasks();

        // Cleanup
        File.Delete(tempFile);

        // Assert
        Assert.Equal(tasks.First().Title, loaded.First().Title);
    }
}