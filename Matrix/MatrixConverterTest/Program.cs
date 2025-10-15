using Lab_1.MatrixConverter;
using System;

namespace Lab_1.MatrixConverterTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== ТЕСТЫ ДЛЯ ArrayToMatrix ===");
            TestArrayToMatrix();

            Console.WriteLine("\n=== ТЕСТЫ ДЛЯ MatrixToArray ===");
            TestMatrixToArray();

            Console.WriteLine("\n=== ИНТЕГРАЦИОННЫЕ ТЕСТЫ ===");
            TestIntegration();

            Console.WriteLine("\n=== ТЕСТЫ ОШИБОК ===");
            TestErrors();

            Console.WriteLine("\nВсе тесты завершены!");
            Console.ReadKey();
        }

        static void TestArrayToMatrix()
        {
            Console.WriteLine("Тест 1: Массив из 3 элементов");
            double[] array1 = { 1, 2, 3 };
            var matrix1 = ArrayToMatrix.ConvertToMatrix(array1);
            PrintMatrix(matrix1);
            // Ожидаемая матрица:
            // [1, 2]
            // [2, 3]

            Console.WriteLine("\nТест 2: Массив из 6 элементов");
            double[] array2 = { 1, 2, 3, 4, 5, 6 };
            var matrix2 = ArrayToMatrix.ConvertToMatrix(array2);
            PrintMatrix(matrix2);
            // Ожидаемая матрица:
            // [1, 2, 3]
            // [2, 4, 5]
            // [3, 5, 6]
        }

        static void TestMatrixToArray()
        {
            Console.WriteLine("Тест 1: Матрица 2x2");
            double[,] matrix1 = {
                { 1, 2 },
                { 2, 3 }
            };
            var array1 = MatrixToArray.ConvertToArray(matrix1);
            PrintArray(array1);
            // Ожидаемый массив: [1, 2, 3]

            Console.WriteLine("\nТест 2: Матрица 3x3");
            double[,] matrix2 = {
                { 1, 2, 3 },
                { 2, 4, 5 },
                { 3, 5, 6 }
            };
            var array2 = MatrixToArray.ConvertToArray(matrix2);
            PrintArray(array2);
            // Ожидаемый массив: [1, 2, 3, 4, 5, 6]
        }

        static void TestIntegration()
        {
            Console.WriteLine("Тест преобразования туда-обратно:");

            // Исходный массив
            double[] originalArray = { 1, 2, 3, 4, 5, 6 };
            Console.WriteLine("Исходный массив:");
            PrintArray(originalArray);

            // Преобразуем в матрицу
            var matrix = ArrayToMatrix.ConvertToMatrix(originalArray);
            Console.WriteLine("\nПолученная матрица:");
            PrintMatrix(matrix);

            // Преобразуем обратно в массив
            var resultArray = MatrixToArray.ConvertToArray(matrix);
            Console.WriteLine("\nМассив после обратного преобразования:");
            PrintArray(resultArray);

            // Проверяем, что массивы совпадают
            bool areEqual = true;
            for (int i = 0; i < originalArray.Length; i++)
            {
                if (originalArray[i] != resultArray[i])
                {
                    areEqual = false;
                    break;
                }
            }
            Console.WriteLine($"\nМассивы совпадают: {areEqual}");
        }

        static void TestErrors()
        {
            Console.WriteLine("Тест 1: Некорректный размер массива");
            try
            {
                double[] badArray = { 1, 2, 3, 4, 5 }; // 5 элементов - невозможно для симметричной матрицы
                var matrix = ArrayToMatrix.ConvertToMatrix(badArray);
                Console.WriteLine("ОШИБКА: Исключение не было выброшено!");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Поймано ожидаемое исключение: {ex.Message}");
            }

            Console.WriteLine("\nТест 2: Несимметричная матрица");
            try
            {
                double[,] badMatrix = {
                    { 1, 2 },
                    { 3, 4 } // Не симметрично: 2 != 3
                };
                var array = MatrixToArray.ConvertToArray(badMatrix);
                Console.WriteLine("ОШИБКА: Исключение не было выброшено!");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Поймано ожидаемое исключение: {ex.Message}");
            }

            Console.WriteLine("\nТест 3: Неквадратная матрица");
            try
            {
                double[,] badMatrix = {
                    { 1, 2, 3 },
                    { 4, 5, 6 }
                };
                var array = MatrixToArray.ConvertToArray(badMatrix);
                Console.WriteLine("ОШИБКА: Исключение не было выброшено!");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Поймано ожидаемое исключение: {ex.Message}");
            }
        }

        static void PrintMatrix(double[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                Console.Write("[");
                for (int j = 0; j < cols; j++)
                {
                    Console.Write(matrix[i, j]);
                    if (j < cols - 1) Console.Write(", ");
                }
                Console.WriteLine("]");
            }
        }

        static void PrintArray(double[] array)
        {
            Console.Write("[");
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write(array[i]);
                if (i < array.Length - 1) Console.Write(", ");
            }
            Console.WriteLine("]");
        }
    }
}