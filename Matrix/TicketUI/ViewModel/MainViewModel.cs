using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using TicketGenerator;

namespace TicketUI.ViewModel
{
    /// <summary>
    /// Main ViewModel для приложения генерации билетов
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly Random _random = new Random();
        private GenerationSettings _settings;
        private ObservableCollection<Ticket> _tickets;
        private string _statusMessage;
        private bool _isGenerating;
        private List<Task> _availableTasks;
        private Ticket _selectedTicket;
        private string _searchText;
        private ICollectionView _ticketsView;

        public MainViewModel()
        {
            _tickets = new ObservableCollection<Ticket>();
            _availableTasks = new List<Task>();
            _settings = new GenerationSettings();

            InitializeCommands();
            SetupTicketsView();
            LoadSampleData();

            StatusMessage = "Готов к работе. Загружено тестовых заданий: " + AvailableTasks.Count;
        }

        private void InitializeCommands()
        {
            GenerateCommand = new RelayCommand(GenerateTickets, CanGenerateTickets);
            ExportCommand = new RelayCommand(ExportTickets, CanExportTickets);
            ClearCommand = new RelayCommand(ClearTickets, CanClearTickets);
            LoadSampleDataCommand = new RelayCommand(LoadSampleData);
        }

        private void SetupTicketsView()
        {
            _ticketsView = CollectionViewSource.GetDefaultView(Tickets);
            _ticketsView.Filter = FilterTickets;
        }

        #region Properties

        public GenerationSettings Settings
        {
            get => _settings;
            set
            {
                _settings = value;
                OnPropertyChanged(nameof(Settings));
            }
        }

        public ObservableCollection<Ticket> Tickets
        {
            get => _tickets;
            set
            {
                _tickets = value;
                OnPropertyChanged(nameof(Tickets));
                UpdateStatistics();
            }
        }

        public Ticket SelectedTicket
        {
            get => _selectedTicket;
            set
            {
                _selectedTicket = value;
                OnPropertyChanged(nameof(SelectedTicket));
                OnPropertyChanged(nameof(IsTicketSelected));
            }
        }

        public bool IsTicketSelected => SelectedTicket != null;

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged(nameof(StatusMessage));
            }
        }

        public bool IsGenerating
        {
            get => _isGenerating;
            set
            {
                _isGenerating = value;
                OnPropertyChanged(nameof(IsGenerating));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public List<Task> AvailableTasks
        {
            get => _availableTasks;
            set
            {
                _availableTasks = value;
                OnPropertyChanged(nameof(AvailableTasks));
                OnPropertyChanged(nameof(AvailableTasksCount));
            }
        }

        public int AvailableTasksCount => AvailableTasks?.Count ?? 0;

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                _ticketsView?.Refresh();
            }
        }

        #endregion

        #region Computed Properties

        public int TotalTasks => Tickets.Sum(ticket => ticket.Tasks?.Count ?? 0);

        public double AverageComplexity => Tickets.Count > 0 ? Tickets.Average(t => t.TotalComplexity) : 0;

        public int MinComplexity => Tickets.Count > 0 ? Tickets.Min(t => t.TotalComplexity) : 0;

        public int MaxComplexity => Tickets.Count > 0 ? Tickets.Max(t => t.TotalComplexity) : 0;

        public string TypeStatistics
        {
            get
            {
                if (Tickets.Count == 0) return "Нет данных";

                var allTasks = Tickets.SelectMany(t => t.Tasks);
                var practiceCount = allTasks.Count(t => t.Type == TaskType.Practice);
                var lectureCount = allTasks.Count(t => t.Type == TaskType.Lecture);
                var blitzCount = allTasks.Count(t => t.Type == TaskType.Blitz);

                return $"Практика: {practiceCount}, Лекции: {lectureCount}, Блиц: {blitzCount}";
            }
        }

        #endregion

        #region Commands

        public ICommand GenerateCommand { get; private set; }
        public ICommand ExportCommand { get; private set; }
        public ICommand ClearCommand { get; private set; }
        public ICommand LoadSampleDataCommand { get; private set; }

        #endregion

        #region Command Methods

        private bool CanGenerateTickets()
        {
            return !IsGenerating &&
                   AvailableTasksCount >= Settings.TasksPerTicket &&
                   Settings.TicketsCount > 0 &&
                   Settings.TasksPerTicket > 0;
        }

        private async void GenerateTickets()
        {
            IsGenerating = true;
            StatusMessage = "Начинаю генерацию билетов...";

            try
            {
                var generatedTickets = await System.Threading.Tasks.Task.Run(() =>
                {
                    var generator = new TicketGeneratorAlgorithm(AvailableTasks, Settings);
                    return generator.GenerateTickets();
                });

                Tickets.Clear();
                foreach (var ticket in generatedTickets)
                {
                    Tickets.Add(ticket);
                }

                UpdateStatistics();
                StatusMessage = $"Успешно сгенерировано {generatedTickets.Count} билетов. " +
                              $"Сложность: {MinComplexity}-{MaxComplexity} (средняя: {AverageComplexity:F1})";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка при генерации билетов: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"Generation error: {ex}");
            }
            finally
            {
                IsGenerating = false;
            }
        }

        private bool CanExportTickets()
        {
            return Tickets.Count > 0;
        }

        private void ExportTickets()
        {
            try
            {
                // Для работы этого кода нужно добавить ссылку на Microsoft.Win32
                /*
                var saveDialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*",
                    FileName = $"Билеты_{DateTime.Now:yyyyMMdd_HHmmss}.txt"
                };

                if (saveDialog.ShowDialog() == true)
                {
                    var exporter = new TicketExporter();
                    exporter.ExportToFile(Tickets.ToList(), saveDialog.FileName);
                    StatusMessage = $"Билеты успешно экспортированы в файл: {saveDialog.FileName}";
                }
                */
                StatusMessage = "Экспорт в файл будет реализован в следующей версии";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка при экспорте: {ex.Message}";
            }
        }

        private bool CanClearTickets()
        {
            return Tickets.Count > 0;
        }

        private void ClearTickets()
        {
            Tickets.Clear();
            SelectedTicket = null;
            UpdateStatistics();
            StatusMessage = "Все билеты очищены";
        }

        private void LoadSampleData()
        {
            try
            {
                var sampleGenerator = new SampleDataGenerator();
                AvailableTasks = sampleGenerator.GenerateSampleTasks(150);
                StatusMessage = $"Загружено {AvailableTasksCount} тестовых заданий";
                UpdateStatistics();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка при загрузке тестовых данных: {ex.Message}";
            }
        }

        #endregion

        #region Helper Methods

        private void UpdateStatistics()
        {
            OnPropertyChanged(nameof(TotalTasks));
            OnPropertyChanged(nameof(AverageComplexity));
            OnPropertyChanged(nameof(MinComplexity));
            OnPropertyChanged(nameof(MaxComplexity));
            OnPropertyChanged(nameof(TypeStatistics));
            OnPropertyChanged(nameof(AvailableTasksCount));
        }

        private bool FilterTickets(object obj)
        {
            if (string.IsNullOrWhiteSpace(SearchText) || !(obj is Ticket ticket))
                return true;

            var searchLower = SearchText.ToLower();

            if (ticket.Id.ToString().Contains(searchLower))
                return true;

            if (ticket.Tasks.Any(task =>
                task.Title.ToLower().Contains(searchLower) ||
                task.Topic.ToLower().Contains(searchLower) ||
                task.Type.ToString().ToLower().Contains(searchLower)))
                return true;

            return false;
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

    /// <summary>
    /// Класс для экспорта билетов
    /// </summary>
    public class TicketExporter
    {
        public void ExportToFile(List<Ticket> tickets, string filePath)
        {
            using (var writer = new System.IO.StreamWriter(filePath, false, System.Text.Encoding.UTF8))
            {
                writer.WriteLine("ЭКЗАМЕНАЦИОННЫЕ БИЛЕТЫ");
                writer.WriteLine($"Дата генерации: {DateTime.Now:dd.MM.yyyy HH:mm}");
                writer.WriteLine($"Всего билетов: {tickets.Count}");
                writer.WriteLine(new string('=', 50));
                writer.WriteLine();

                foreach (var ticket in tickets)
                {
                    writer.WriteLine($"БИЛЕТ №{ticket.Id}");
                    writer.WriteLine($"Общая сложность: {ticket.TotalComplexity}");
                    writer.WriteLine(new string('-', 30));

                    for (int i = 0; i < ticket.Tasks.Count; i++)
                    {
                        var task = ticket.Tasks[i];
                        writer.WriteLine($"{i + 1}. {task.Title}");
                        writer.WriteLine($"   Тема: {task.Topic}");
                        writer.WriteLine($"   Тип: {GetTaskTypeName(task.Type)}");
                        writer.WriteLine($"   Сложность: {task.Complexity}");
                        writer.WriteLine();
                    }
                    writer.WriteLine(new string('=', 50));
                    writer.WriteLine();
                }
            }
        }

        private string GetTaskTypeName(TaskType type)
        {
            switch (type)
            {
                case TaskType.Practice:
                    return "Практическое задание";
                case TaskType.Lecture:
                    return "Теоретический вопрос";
                case TaskType.Blitz:
                    return "Блиц-вопрос";
                default:
                    return "Неизвестный тип";
            }
        }
    }

    /// <summary>
    /// Генератор тестовых данных
    /// </summary>
    public class SampleDataGenerator
    {
        private readonly Random _random = new Random();
        private readonly string[] _topics = {
            "ООП и классы", "Алгоритмы и структуры данных", "Базы данных и SQL",
            "Сетевые технологии", "Web разработка", "Безопасность программ",
            "Паттерны проектирования", "Тестирование ПО", "Архитектура приложений",
            "Многопоточное программирование", "LINQ и коллекции", "ASP.NET Core"
        };

        private readonly string[] _practiceTitles = {
            "Реализация класса {0}", "Написание метода для {0}", "Создание алгоритма {0}",
            "Оптимизация кода {0}", "Отладка программы {0}", "Рефакторинг {0}"
        };

        private readonly string[] _lectureTitles = {
            "Принципы {0}", "Теория {0}", "Концепции {0}", "Основы {0}",
            "Сравнение подходов {0}", "История развития {0}"
        };

        private readonly string[] _blitzTitles = {
            "Быстрый вопрос: {0}", "Определение: {0}", "Краткая характеристика {0}",
            "Основное понятие {0}", "Блиц: {0}"
        };

        public List<Task> GenerateSampleTasks(int count)
        {
            var tasks = new List<Task>();

            for (int i = 1; i <= count; i++)
            {
                var topic = _topics[_random.Next(_topics.Length)];
                var taskType = (TaskType)_random.Next(3);

                tasks.Add(new Task
                {
                    Id = i,
                    Title = GenerateTaskTitle(taskType, topic),
                    Type = taskType,
                    Topic = topic,
                    Complexity = GenerateComplexity(taskType)
                });
            }

            return tasks;
        }

        private string GenerateTaskTitle(TaskType type, string topic)
        {
            string format;
            switch (type)
            {
                case TaskType.Practice:
                    format = _practiceTitles[_random.Next(_practiceTitles.Length)];
                    break;
                case TaskType.Lecture:
                    format = _lectureTitles[_random.Next(_lectureTitles.Length)];
                    break;
                case TaskType.Blitz:
                    format = _blitzTitles[_random.Next(_blitzTitles.Length)];
                    break;
                default:
                    format = "Задание по {0}";
                    break;
            }
            return string.Format(format, topic);
        }

        private int GenerateComplexity(TaskType type)
        {
            switch (type)
            {
                case TaskType.Practice:
                    return _random.Next(3, 11);
                case TaskType.Lecture:
                    return _random.Next(2, 8);
                case TaskType.Blitz:
                    return _random.Next(1, 5);
                default:
                    return _random.Next(1, 6);
            }
        }
    }
}