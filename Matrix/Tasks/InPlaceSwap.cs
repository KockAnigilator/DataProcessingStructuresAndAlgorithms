using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_3.Tasks
{
    /// <summary>
    /// Алгоритмы обмена элементов без использования дополнительной памяти
    /// </summary>
    public class InPlaceSwap
    {
        /// <summary>
        /// Обмен с использованием арифметических операций (опасно при переполнении)
        /// </summary>
        public static void ArithmeticSwap(ref int a, ref int b)
        {
            Console.WriteLine($"До обмена: a = {a}, b = {b}");

            a = a + b;  // a теперь содержит сумму
            b = a - b;  // b получает исходное значение a
            a = a - b;  // a получает исходное значение b

            Console.WriteLine($"После арифметического обмена: a = {a}, b = {b}");
        }

        /// <summary>
        /// Обмен с использованием XOR (безопаснее)
        /// </summary>
        public static void XorSwap(ref int a, ref int b)
        {
            Console.WriteLine($"До обмена: a = {a}, b = {b}");

            a = a ^ b;  // a содержит XOR a и b
            b = a ^ b;  // b получает исходное a
            a = a ^ b;  // a получает исходное b

            Console.WriteLine($"После XOR обмена: a = {a}, b = {b}");
        }

        /// <summary>
        /// Обмен элементов массива с использованием арифметики
        /// </summary>
        public static void SwapArrayElements(int[] arr, int index1, int index2)
        {
            if (arr == null || index1 < 0 || index2 < 0 ||
                index1 >= arr.Length || index2 >= arr.Length)
                throw new ArgumentException("Некорректные параметры");

            Console.WriteLine($"Массив до обмена: [{string.Join(", ", arr)}]");
            Console.WriteLine($"Обмен элементов с индексами {index1} и {index2}");

            // Используем арифметический метод
            arr[index1] = arr[index1] + arr[index2];
            arr[index2] = arr[index1] - arr[index2];
            arr[index1] = arr[index1] - arr[index2];

            Console.WriteLine($"Массив после обмена: [{string.Join(", ", arr)}]");
        }
    }
}
