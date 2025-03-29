using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTaskManager
{
    public class ConsoleInputProvider : ITaskInputProvider
    {
        public string GetTitle()
        {
            return GetInput("Введите название задачи:", "Название не может быть пустым!");
        }

        public string GetDescription()
        {
            return GetInput("Введите описание:", "Описание не может быть пустым!");
        }

        public DateTime? GetDueDate()
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
    }
}
