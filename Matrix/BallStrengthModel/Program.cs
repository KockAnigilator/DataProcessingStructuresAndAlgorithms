using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BallStrengthModel
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== Анализ задачи о двух шарах ===");

            // Параметры
            int maxFloors = 100;
            int[] testStrengths = { 1, 25, 50, 75, 100 };

            // Тестируем для разных прочностей
            foreach (int strength in testStrengths)
            {
                int throws = FindThrowsWithJumpSearch(maxFloors, strength);
                Console.WriteLine($"Прочность {strength,3}: {throws,2} бросков");
            }

            // Находим формулу методом наименьших квадратов
            FindFormula();
        }

        // Алгоритм Jump Search для двух шаров
        static int FindThrowsWithJumpSearch(int totalFloors, int actualStrength)
        {
            if (totalFloors <= 0) return 0;

            int throws = 0;
            int jumpSize = (int)Math.Sqrt(totalFloors); // Размер прыжка = √n
            int current = jumpSize;
            int prev = 0;

            // Фаза 1: Прыжки первым шаром
            while (current <= totalFloors && current < actualStrength)
            {
                throws++;
                prev = current;
                current += jumpSize;

                if (current > totalFloors)
                    current = totalFloors;
            }

            // Фаза 2: Линейный поиск вторым шаром
            for (int floor = prev + 1; floor <= current; floor++)
            {
                throws++;
                if (floor >= actualStrength)
                    return throws;
            }

            return throws;
        }

        // Метод наименьших квадратов для нахождения формулы
        static void FindFormula()
        {
            Console.WriteLine("\n=== Нахождение формулы методом наименьших квадратов ===");

            // Данные: n этажей -> количество бросков
            int[] floors = { 10, 15, 20, 25, 30, 35, 40 };
            int[] throws = new int[floors.Length];

            // Для простоты берем худший случай (прочность = n)
            for (int i = 0; i < floors.Length; i++)
            {
                throws[i] = FindThrowsWithJumpSearch(floors[i], floors[i]);
            }

            // Выводим данные
            Console.WriteLine("Данные для МНК:");
            Console.WriteLine("√n\tБроски");
            Console.WriteLine("----------------");
            for (int i = 0; i < floors.Length; i++)
            {
                double sqrtN = Math.Sqrt(floors[i]);
                Console.WriteLine($"{sqrtN,4:F1}\t{throws[i],2}");
            }

            // Метод наименьших квадратов: y = kx + b
            // где x = √n, y = броски
            double sumX = 0, sumY = 0, sumXY = 0, sumX2 = 0;
            int n = floors.Length;

            for (int i = 0; i < n; i++)
            {
                double x = Math.Sqrt(floors[i]);
                double y = throws[i];

                sumX += x;
                sumY += y;
                sumXY += x * y;
                sumX2 += x * x;
            }

            // Вычисляем k и b
            double k = (n * sumXY - sumX * sumY) / (n * sumX2 - sumX * sumX);
            double b = (sumY - k * sumX) / n;

            Console.WriteLine($"\nФормула: броски = {k:F2} * √n + {b:F2}");

            // Проверяем точность
            Console.WriteLine("\nПроверка формулы:");
            Console.WriteLine("n\tРеально\tФормула\tОшибка");
            Console.WriteLine("------------------------------");
            for (int i = 0; i < floors.Length; i++)
            {
                double real = throws[i];
                double predicted = k * Math.Sqrt(floors[i]) + b;
                double error = Math.Abs(real - predicted);
                Console.WriteLine($"{floors[i]}\t{real}\t{predicted,5:F1}\t{error,4:F1}");
            }
        }
    }
}
