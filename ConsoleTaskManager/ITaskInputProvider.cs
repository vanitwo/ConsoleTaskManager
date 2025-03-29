using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTaskManager;

public interface ITaskInputProvider
{
    string GetTitle();
    string GetDescription();
    DateTime? GetDueDate();
}
