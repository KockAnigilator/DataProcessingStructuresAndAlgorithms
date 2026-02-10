using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketGenerator
{
    // Класс задания
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public TaskType Type { get; set; }
        public string Topic { get; set; }
        public int Complexity { get; set; } // Сложность от 1 до 10
    }
}
