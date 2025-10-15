using Lab_3.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Lab_3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== ТЕСТИРОВАНИЕ АЛГОРИТМОВ ===\n");

            TestEggDroppingProblem();
            TestInPlaceSwap();
            TestInsertionSorts();
            TestPerformanceComparison();

            Console.WriteLine("\nВсе тесты завершены!");
            Console.ReadKey();
        }

        static void TestEggDroppingProblem()
        {
            Console.WriteLine("1. ЗАДАЧА О ШАРАХ:");

            int[] testFloors = { 10, 36, 100, 500, 1000 };

            foreach (int floors in testFloors)
            {
                int attempts = EggDroppingSolver.FindMinimumAttempts(floors);
                var sequence = EggDroppingSolver.FindOptimalSequence(floors);
                int dpAttempts = EggDroppingSolver.FindMinimumAttemptsDP(floors, 2);

                Console.WriteLine($"\n{floors} этажей:");
                Console.WriteLine($"Формула: {attempts} попыток");
                Console.WriteLine($"ДП: {dpAttempts} попыток");
                Console.WriteLine($"Оптимальная последовательность: {string.Join(" → ", sequence)}");

                // Проверяем, что оба метода дают одинаковый результат
                if (attempts != dpAttempts)
                {
                    Console.WriteLine($"ОШИБКА: Методы дают разные результаты!");
                }
            }

            // Тестируем для разного количества шаров
            Console.WriteLine("\n--- Разное количество шаров ---");
            Console.WriteLine($"3 шара, 14 этажей: {EggDroppingSolver.FindMinimumAttemptsDP(14, 3)} попыток");
            Console.WriteLine($"4 шара, 100 этажей: {EggDroppingSolver.FindMinimumAttemptsDP(100, 4)} попыток");
        }

        static void TestInPlaceSwap()
        {
            Console.WriteLine("\n\n2. ОБМЕН ЭЛЕМЕНТОВ БЕЗ ДОПОЛНИТЕЛЬНОЙ ПАМЯТИ:");

            // Тестируем арифметический обмен
            int a = 5, b = 10;
            Console.WriteLine("\n--- Арифметический обмен ---");
            InPlaceSwap.ArithmeticSwap(ref a, ref b);

            // Проверяем результат
            if (a == 10 && b == 5)
                Console.WriteLine("✓ Обмен выполнен корректно");
            else
                Console.WriteLine("✗ Ошибка в обмене");

            // Тестируем XOR обмен
            a = 15; b = 25;
            Console.WriteLine("\n--- XOR обмен ---");
            InPlaceSwap.XorSwap(ref a, ref b);

            if (a == 25 && b == 15)
                Console.WriteLine("✓ Обмен выполнен корректно");
            else
                Console.WriteLine("✗ Ошибка в обмене");

            // Тестируем обмен в массиве
            int[] arr = { 1, 2, 3, 4, 5 };
            Console.WriteLine("\n--- Обмен в массиве ---");
            InPlaceSwap.SwapArrayElements(arr, 1, 3);

            if (arr[1] == 4 && arr[3] == 2)
                Console.WriteLine("✓ Обмен в массиве выполнен корректно");
            else
                Console.WriteLine("✗ Ошибка в обмене в массиве");

            // Тестируем граничные случаи
            try
            {
                InPlaceSwap.SwapArrayElements(null, 0, 1);
                Console.WriteLine("✗ Ожидалось исключение для null массива");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("✓ Корректно обработано исключение для null массива");
            }
        }

        static void TestInsertionSorts()
        {
            Console.WriteLine("\n\n3. СОРТИРОВКА ВСТАВКАМИ:");

            int[] testArray1 = { 5, 2, 8, 1, 9, 3 };
            int[] testArray2 = new int[testArray1.Length];
            Array.Copy(testArray1, testArray2, testArray1.Length);

            Console.WriteLine($"Исходный массив: [{string.Join(", ", testArray1)}]");

            // Классическая сортировка
            InsertionSort.ClassicInsertionSort(testArray1);
            Console.WriteLine($"Классическая сортировка: [{string.Join(", ", testArray1)}]");
            Console.WriteLine($"Корректно отсортирован: {InsertionSort.IsSorted(testArray1)}");

            // Сортировка с бинарным поиском
            InsertionSort.BinaryInsertionSort(testArray2);
            Console.WriteLine($"С бинарным поиском: [{string.Join(", ", testArray2)}]");
            Console.WriteLine($"Корректно отсортирован: {InsertionSort.IsSorted(testArray2)}");

            // Проверяем, что результаты идентичны
            bool arraysEqual = testArray1.SequenceEqual(testArray2);
            Console.WriteLine($"Результаты идентичны: {arraysEqual}");

            // Тестируем на большем массиве
            int[] largeArray = InsertionSort.GenerateRandomArray(20);
            int[] largeArrayCopy = new int[20];
            Array.Copy(largeArray, largeArrayCopy, 20);

            InsertionSort.ClassicInsertionSort(largeArray);
            InsertionSort.BinaryInsertionSort(largeArrayCopy);

            Console.WriteLine($"\nБольшой массив - оба метода работают корректно: " +
                            $"{InsertionSort.IsSorted(largeArray) && InsertionSort.IsSorted(largeArrayCopy)}");
        }

        static void TestPerformanceComparison()
        {
            Console.WriteLine("\n\n4. СРАВНЕНИЕ ПРОИЗВОДИТЕЛЬНОСТИ:");

            // Запускаем сравнение производительности
            InsertionSort.ComparePerformance();

            // Дополнительный тест для определения точки эффективности
            Console.WriteLine("\n--- ОПРЕДЕЛЕНИЕ ТОЧКИ ЭФФЕКТИВНОСТИ ---");

            int pointOfEfficiency = FindPointOfEfficiency();
            Console.WriteLine($"\n*** ВЫВОД: Бинарная сортировка становится эффективнее классической начиная с ~{pointOfEfficiency} элементов ***");
        }

        static int FindPointOfEfficiency()
        {
            // Ищем точку, где бинарная версия становится эффективнее
            for (int size = 10; size <= 1000; size += 10)
            {
                int[] arr1 = InsertionSort.GenerateRandomArray(size);
                int[] arr2 = new int[size];
                Array.Copy(arr1, arr2, size);

                var stopwatch1 = System.Diagnostics.Stopwatch.StartNew();
                InsertionSort.ClassicInsertionSort(arr1);
                stopwatch1.Stop();

                var stopwatch2 = System.Diagnostics.Stopwatch.StartNew();
                InsertionSort.BinaryInsertionSort(arr2);
                stopwatch2.Stop();

                // Если бинарная версия стала быстрее
                if (stopwatch2.ElapsedTicks < stopwatch1.ElapsedTicks)
                {
                    return size;
                }
            }

            return -1; // Не найдено
        }
    }
}
