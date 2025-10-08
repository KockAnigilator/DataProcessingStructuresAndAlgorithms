using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketGenerator
{
    public class TicketGeneratorAlgorithm
    {
        private List<Task> _allTasks;
        private GenerationSettings _settings;
        private Random _random;

        public TicketGeneratorAlgorithm(List<Task> allTasks, GenerationSettings settings)
        {
            _allTasks = allTasks ?? throw new ArgumentNullException(nameof(allTasks));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _random = new Random();
        }

        /// <summary>
        /// Основной метод генерации билетов
        /// Сложность: O(k * n * m), где k - количество билетов, n - количество заданий в билете, m - общее количество заданий
        /// </summary>
        public List<Ticket> GenerateTickets()
        {
            var tickets = new List<Ticket>();
            var availableTasks = new List<Task>(_allTasks);

            // Группируем задания по темам для равномерного распределения
            var tasksByTopic = availableTasks.GroupBy(t => t.Topic)
                                           .ToDictionary(g => g.Key, g => g.ToList());

            for (int i = 0; i < _settings.TicketsCount; i++)
            {
                var ticket = GenerateSingleTicket(availableTasks, tasksByTopic, i + 1);
                if (ticket != null)
                {
                    tickets.Add(ticket);
                    // Удаляем использованные задания для избежания повторов
                    RemoveUsedTasks(availableTasks, ticket.Tasks);
                    UpdateTopicDistribution(tasksByTopic, ticket.Tasks);
                }
            }

            return tickets;
        }

        /// <summary>
        /// Генерация одного билета
        /// Сложность: O(n * m), где n - количество заданий в билете, m - доступное количество заданий
        /// </summary>
        private Ticket GenerateSingleTicket(List<Task> availableTasks,
                                          Dictionary<string, List<Task>> tasksByTopic,
                                          int ticketId)
        {
            var ticketTasks = new List<Task>();
            var usedTaskIds = new HashSet<int>();

            // Вычисляем целевое количество заданий каждого типа
            var typeDistribution = CalculateTypeDistribution(_settings.TasksPerTicket);

            // Сначала добавляем задания из разных тем
            if (_settings.PreferDiverseTopics && tasksByTopic.Count >= typeDistribution.Count)
            {
                AddTasksFromDifferentTopics(ticketTasks, tasksByTopic, usedTaskIds, typeDistribution);
            }

            // Затем добираем оставшиеся задания с учетом баланса сложности
            CompleteTicketWithBalancedComplexity(ticketTasks, availableTasks, usedTaskIds,
                                               typeDistribution, _settings.TargetComplexity);

            // Проверяем соответствие требованиям
            if (!ValidateTicket(ticketTasks, _settings.TasksPerTicket, _settings.TargetComplexity,
                              _settings.ComplexityTolerance))
            {
                return TryRegenerateTicket(availableTasks, tasksByTopic, ticketId);
            }

            return new Ticket { Id = ticketId, Tasks = ticketTasks };
        }

        /// <summary>
        /// Расчет распределения типов заданий
        /// Сложность: O(1)
        /// </summary>
        private Dictionary<TaskType, int> CalculateTypeDistribution(int totalTasks)
        {
            var distribution = new Dictionary<TaskType, int>();

            if (_settings.BalanceTaskTypes)
            {
                distribution[TaskType.Practice] = (int)Math.Round(totalTasks * _settings.PracticePercentage / 100);
                distribution[TaskType.Lecture] = (int)Math.Round(totalTasks * _settings.LecturePercentage / 100);
                distribution[TaskType.Blitz] = totalTasks - distribution[TaskType.Practice] - distribution[TaskType.Lecture];
            }
            else
            {
                // Равномерное распределение
                var baseCount = totalTasks / 3;
                distribution[TaskType.Practice] = baseCount;
                distribution[TaskType.Lecture] = baseCount;
                distribution[TaskType.Blitz] = totalTasks - 2 * baseCount;
            }

            return distribution;
        }

        /// <summary>
        /// Добавление заданий из разных тем
        /// Сложность: O(t * n), где t - количество тем, n - среднее количество заданий в теме
        /// </summary>
        private void AddTasksFromDifferentTopics(List<Task> ticketTasks,
                                               Dictionary<string, List<Task>> tasksByTopic,
                                               HashSet<int> usedTaskIds,
                                               Dictionary<TaskType, int> typeDistribution)
        {
            var topics = tasksByTopic.Keys.ToList();
            var currentTopicIndex = 0;
            var types = typeDistribution.Keys.ToArray();
            var currentTypeIndex = 0;

            while (ticketTasks.Count < _settings.TasksPerTicket &&
                   currentTopicIndex < topics.Count &&
                   currentTypeIndex < types.Length)
            {
                var topic = topics[currentTopicIndex];
                var targetType = types[currentTypeIndex];

                if (typeDistribution[targetType] > 0)
                {
                    var task = FindTaskByTypeAndTopic(tasksByTopic, topic, targetType, usedTaskIds);
                    if (task != null)
                    {
                        ticketTasks.Add(task);
                        usedTaskIds.Add(task.Id);
                        typeDistribution[targetType]--;
                    }
                }

                currentTypeIndex++;
                if (currentTypeIndex >= types.Length)
                {
                    currentTypeIndex = 0;
                    currentTopicIndex++;
                }
            }
        }

        /// <summary>
        /// Поиск задания по типу и теме
        /// Сложность: O(n) в худшем случае
        /// </summary>
        private Task FindTaskByTypeAndTopic(Dictionary<string, List<Task>> tasksByTopic,
                                          string topic, TaskType type, HashSet<int> usedTaskIds)
        {
            if (!tasksByTopic.ContainsKey(topic))
                return null;

            var availableTasks = tasksByTopic[topic]
                .Where(t => t.Type == type && !usedTaskIds.Contains(t.Id))
                .ToList();

            return availableTasks.Count > 0 ? availableTasks[_random.Next(availableTasks.Count)] : null;
        }

        /// <summary>
        /// Завершение формирования билета с балансировкой сложности
        /// Сложность: O(n * m), где n - количество оставшихся заданий, m - доступные задания
        /// </summary>
        private void CompleteTicketWithBalancedComplexity(List<Task> ticketTasks,
                                                        List<Task> availableTasks,
                                                        HashSet<int> usedTaskIds,
                                                        Dictionary<TaskType, int> typeDistribution,
                                                        int targetComplexity)
        {
            int currentComplexity = ticketTasks.Sum(t => t.Complexity);
            int remainingSlots = _settings.TasksPerTicket - ticketTasks.Count;

            for (int i = 0; i < remainingSlots && ticketTasks.Count < _settings.TasksPerTicket; i++)
            {
                var neededComplexity = (targetComplexity - currentComplexity) / (remainingSlots - i);
                var task = FindOptimalTask(availableTasks, usedTaskIds, typeDistribution, neededComplexity);

                if (task != null)
                {
                    ticketTasks.Add(task);
                    usedTaskIds.Add(task.Id);
                    currentComplexity += task.Complexity;
                    typeDistribution[task.Type]--;
                }
                else
                {
                    // Если не нашли оптимальное задание, берем любое подходящее
                    task = FindAnySuitableTask(availableTasks, usedTaskIds, typeDistribution);
                    if (task != null)
                    {
                        ticketTasks.Add(task);
                        usedTaskIds.Add(task.Id);
                        currentComplexity += task.Complexity;
                        typeDistribution[task.Type]--;
                    }
                }
            }
        }

        /// <summary>
        /// Поиск оптимального задания по сложности и типу
        /// Сложность: O(m), где m - количество доступных заданий
        /// </summary>
        private Task FindOptimalTask(List<Task> availableTasks, HashSet<int> usedTaskIds,
                                   Dictionary<TaskType, int> typeDistribution, int neededComplexity)
        {
            var suitableTasks = availableTasks
                .Where(t => !usedTaskIds.Contains(t.Id) && typeDistribution[t.Type] > 0)
                .OrderBy(t => Math.Abs(t.Complexity - neededComplexity))
                .ThenBy(t => _random.Next()) // Добавляем случайность для разнообразия
                .ToList();

            return suitableTasks.FirstOrDefault();
        }

        /// <summary>
        /// Поиск любого подходящего задания
        /// Сложность: O(m)
        /// </summary>
        private Task FindAnySuitableTask(List<Task> availableTasks, HashSet<int> usedTaskIds,
                                       Dictionary<TaskType, int> typeDistribution)
        {
            return availableTasks
                .FirstOrDefault(t => !usedTaskIds.Contains(t.Id) && typeDistribution[t.Type] > 0);
        }

        /// <summary>
        /// Валидация билета
        /// Сложность: O(n)
        /// </summary>
        private bool ValidateTicket(List<Task> tasks, int expectedCount, int targetComplexity, int tolerance)
        {
            if (tasks.Count != expectedCount)
                return false;

            int totalComplexity = tasks.Sum(t => t.Complexity);
            if (Math.Abs(totalComplexity - targetComplexity) > tolerance)
                return false;

            // Проверяем разнообразие тем (минимум 2 разные темы)
            var distinctTopics = tasks.Select(t => t.Topic).Distinct().Count();
            if (distinctTopics < 2)
                return false;

            return true;
        }

        /// <summary>
        /// Попытка перегенерации билета при неудаче
        /// Сложность: O(n * m)
        /// </summary>
        private Ticket TryRegenerateTicket(List<Task> availableTasks,
                                         Dictionary<string, List<Task>> tasksByTopic,
                                         int ticketId)
        {
            // Упрощенная стратегия: выбираем случайные задания с проверкой сложности
            for (int attempt = 0; attempt < 100; attempt++) // Ограничиваем количество попыток
            {
                var candidateTasks = availableTasks
                    .OrderBy(x => _random.Next())
                    .Take(_settings.TasksPerTicket)
                    .ToList();

                if (ValidateTicket(candidateTasks, _settings.TasksPerTicket,
                                 _settings.TargetComplexity, _settings.ComplexityTolerance))
                {
                    return new Ticket { Id = ticketId, Tasks = candidateTasks };
                }
            }

            return null; // Не удалось сгенерировать валидный билет
        }

        private void RemoveUsedTasks(List<Task> availableTasks, List<Task> usedTasks)
        {
            foreach (var task in usedTasks)
            {
                availableTasks.RemoveAll(t => t.Id == task.Id);
            }
        }

        private void UpdateTopicDistribution(Dictionary<string, List<Task>> tasksByTopic, List<Task> usedTasks)
        {
            foreach (var task in usedTasks)
            {
                if (tasksByTopic.ContainsKey(task.Topic))
                {
                    tasksByTopic[task.Topic].RemoveAll(t => t.Id == task.Id);
                }
            }
        }
    }
}
