using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketGenerator
{
    // Внешние условия (настройки генерации)
    public class GenerationSettings
    {
        public int TicketsCount { get; set; } = 10;
        public int TasksPerTicket { get; set; } = 5;
        public int TargetComplexity { get; set; } = 20;
        public int ComplexityTolerance { get; set; } = 2;
        public bool PreferDiverseTopics { get; set; } = true;
        public bool BalanceTaskTypes { get; set; } = true;

        // Распределение типов заданий (в процентах)
        public double PracticePercentage { get; set; } = 40;
        public double LecturePercentage { get; set; } = 40;
        public double BlitzPercentage { get; set; } = 20;
    }
}
