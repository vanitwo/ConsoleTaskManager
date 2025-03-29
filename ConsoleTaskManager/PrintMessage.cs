using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTaskManager
{
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
}
