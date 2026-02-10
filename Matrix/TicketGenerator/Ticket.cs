using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketGenerator
{
    // Класс билета
    public class Ticket
    {
        public int Id { get; set; }
        public List<Task> Tasks { get; set; } = new List<Task>();
        public int TotalComplexity => Tasks.Sum(t => t.Complexity);

        public override string ToString()
        {
            return $"Билет {Id}: {Tasks.Count} заданий, сложность {TotalComplexity}";
        }
    }
}
