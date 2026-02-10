using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_3.Tasks
{
    /// <summary>
    /// Реализация различных вариантов сортировки вставками
    /// </summary>
    public class InsertionSort
    {
        /// <summary>
        /// Классическая сортировка вставками
        /// </summary>
        public static void ClassicInsertionSort(int[] arr)
        {
            if (arr == null || arr.Length <= 1) return;

            for (int i = 1; i < arr.Length; i++)
            {
                int key = arr[i];
                int j = i - 1;

                // Сдвигаем элементы большие key
                while (j >= 0 && arr[j] > key)
                {
                    arr[j + 1] = arr[j];
                    j--;
                }
                arr[j + 1] = key;
            }
        }

        /// <summary>
        /// Сортировка вставками с бинарным поиском
        /// </summary>
        public static void BinaryInsertionSort(int[] arr)
        {
            if (arr == null || arr.Length <= 1) return;

            for (int i = 1; i < arr.Length; i++)
            {
                int key = arr[i];

                // Находим позицию для вставки с помощью бинарного поиска
                int pos = BinarySearch(arr, 0, i - 1, key);

                // Сдвигаем элементы
                for (int j = i - 1; j >= pos; j--)
                {
                    arr[j + 1] = arr[j];
                }

                arr[pos] = key;
            }
        }

        /// <summary>
        /// Бинарный поиск позиции для вставки
        /// </summary>
        private static int BinarySearch(int[] arr, int left, int right, int key)
        {
            while (left <= right)
            {
                int mid = left + (right - left) / 2;

                if (arr[mid] == key)
                    return mid;
                else if (arr[mid] < key)
                    left = mid + 1;
                else
                    right = mid - 1;
            }

            return left;
        }

        /// <summary>
        /// Тестирование производительности разных алгоритмов сортировки
        /// </summary>
        public static void ComparePerformance()
        {
            Console.WriteLine("\n=== СРАВНЕНИЕ ПРОИЗВОДИТЕЛЬНОСТИ ===");

            int[] testSizes = { 10, 50, 100, 500, 1000, 5000, 10000 };

            foreach (int size in testSizes)
            {
                Console.WriteLine($"\n--- Размер массива: {size} ---");

                // Создаем идентичные массивы для тестирования
                int[] arr1 = GenerateRandomArray(size);
                int[] arr2 = new int[size];
                Array.Copy(arr1, arr2, size);

                // Тестируем классическую сортировку
                var stopwatch1 = Stopwatch.StartNew();
                ClassicInsertionSort(arr1);
                stopwatch1.Stop();

                // Тестируем сортировку с бинарным поиском
                var stopwatch2 = Stopwatch.StartNew();
                BinaryInsertionSort(arr2);
                stopwatch2.Stop();

                Console.WriteLine($"Классическая: {stopwatch1.ElapsedTicks} тиков");
                Console.WriteLine($"С бинарным поиском: {stopwatch2.ElapsedTicks} тиков");

                if (stopwatch1.ElapsedTicks < stopwatch2.ElapsedTicks)
                {
                    Console.WriteLine($"Классическая быстрее на {stopwatch2.ElapsedTicks - stopwatch1.ElapsedTicks} тиков");
                }
                else
                {
                    Console.WriteLine($"С бинарным поиском быстрее на {stopwatch1.ElapsedTicks - stopwatch2.ElapsedTicks} тиков");
                }

                // Определяем точку эффективности
                if (size >= 100 && stopwatch2.ElapsedTicks < stopwatch1.ElapsedTicks)
                {
                    Console.WriteLine($"*** Бинарная версия становится эффективнее начиная с {size} элементов ***");
                }
            }
        }

        /// <summary>
        /// Генерация случайного массива
        /// </summary>
        public static int[] GenerateRandomArray(int size)
        {
            Random random = new Random();
            int[] arr = new int[size];

            for (int i = 0; i < size; i++)
            {
                arr[i] = random.Next(1, 1000);
            }

            return arr;
        }

        /// <summary>
        /// Проверка корректности сортировки
        /// </summary>
        public static bool IsSorted(int[] arr)
        {
            for (int i = 1; i < arr.Length; i++)
            {
                if (arr[i] < arr[i - 1])
                    return false;
            }
            return true;
        }
    }
}
