using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTaskManager
{
    public class TaskItem
    {
        public static int _idCounter = 1;
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }

        public TaskItem() { }

        public TaskItem(string title, string description, DateTime dueDate)
        {
            Id = _idCounter++;
            Title = title;
            Description = description;
            DueDate = dueDate;
            IsCompleted = false;
        }

        public ConsoleColor GetTaskColor()
        {
            if (IsCompleted) return ConsoleColor.Green;
            if (DueDate < DateTime.Now) return ConsoleColor.Red;
            return ConsoleColor.Blue;
        }                
    }
}
