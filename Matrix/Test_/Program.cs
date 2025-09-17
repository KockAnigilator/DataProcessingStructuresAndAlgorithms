using Matrix.Lab_3;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_
{
    internal class Program
    {
        static void Main()
        {
            //// Диапазон размеров массивов для тестирования
            //int minSize = 10;
            //int maxSize = 1000;
            //int step = 10;
            //int trials = 5; // Количество испытаний для каждого размера



            //Console.WriteLine("Размер | Классическая (мс) | Бинарная (мс) | Выигрыш");
            //Console.WriteLine("---------------------------------------------------");
            //Task_3 task_3 = new Task_3();

            //for (int size = minSize; size <= maxSize; size += step)
            //{
            //    double classicTotalTime = 0;
            //    double binaryTotalTime = 0;

            //    for (int trial = 0; trial < trials; trial++)
            //    {
            //        int[] original = task_3.GenerateRandomArray(size);
            //        int[] classicArray = task_3.CopyArray(original);
            //        int[] binaryArray = task_3.CopyArray(original);

            //        // Замер времени для классической сортировки
            //        Stopwatch classicStopwatch = Stopwatch.StartNew();
            //        task_3.ClassicInsertionSort(classicArray);
            //        classicStopwatch.Stop();
            //        classicTotalTime += classicStopwatch.Elapsed.TotalMilliseconds;

            //        // Замер времени для бинарной сортировки
            //        Stopwatch binaryStopwatch = Stopwatch.StartNew();
            //        task_3.BinaryInsertionSort(binaryArray);
            //        binaryStopwatch.Stop();
            //        binaryTotalTime += binaryStopwatch.Elapsed.TotalMilliseconds;
            //    }

            //    double classicAvgTime = classicTotalTime / trials;
            //    double binaryAvgTime = binaryTotalTime / trials;

            //    string advantage = classicAvgTime > binaryAvgTime ? "ДА" : "нет";

            //    Console.WriteLine($"{size,5} | {classicAvgTime,15:F4} | {binaryAvgTime,12:F4} | {advantage}");

            //    // Остановиться, когда бинарная версия станет consistently быстрее
            //    if (classicAvgTime > binaryAvgTime && size > 100)
            //    {
            //        Console.WriteLine($"\nБинарная сортировка становится эффективнее начиная с ~{size} элементов");
            //        break;
            //    }
            //}


            Console.Write("Введите количество этажей n: ");
            string input = Console.ReadLine();

            Task_1 task_1 = new Task_1();
            if (int.TryParse(input, out int n) && n >= 0)
            {
                int result = task_1.Throws(n);
                Console.WriteLine($"Минимальное количество бросков для {n}-этажного здания: {result}");
            }
            else
            {
                Console.WriteLine("Ошибка: введите целое неотрицательное число.");
            }
        }
    }
}
