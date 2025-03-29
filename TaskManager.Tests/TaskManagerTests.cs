using ConsoleTaskManager;
using Moq;

namespace TaskManagers.Tests;

public class TaskManagerTests : IDisposable
{
    private readonly Mock<JsonFileService> _mockFileService;
    private readonly Mock<ITaskInputProvider> _mockInput;
    private readonly TaskManager _taskManager;

    public TaskManagerTests()
    {
        _mockFileService = new Mock<JsonFileService>();
        _mockInput = new Mock<ITaskInputProvider>();
        _taskManager = new TaskManager(
            _mockFileService.Object,
            _mockInput.Object
        );
    }

    public void Dispose()
    {
        TaskItem.ResetIdCounter();
    }
}
