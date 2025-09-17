using Matrix;
using Matrix.Lab_2;
using System;

namespace Matrix
{
    class Program
    {
        static void Lab_2()
        {
            Console.WriteLine("=== ТЕСТИРОВАНИЕ МАТРИЦ ===");

            // Тестируем квадратные матрицы
            Console.WriteLine("\n1. КВАДРАТНЫЕ МАТРИЦЫ:");

            SquareMatrix sq1 = new SquareMatrix(3);
            SquareMatrix sq2 = new SquareMatrix(3);

            sq1.FillRandom(1, 5);
            sq2.FillRandom(1, 5);

            Console.WriteLine("Матрица A:");
            sq1.Print();

            Console.WriteLine("Матрица B:");
            sq2.Print();

            Console.WriteLine("A + B:");
            MatrixP sum = sq1.Add(sq2);
            sum.Print();

            Console.WriteLine("A - B:");
            MatrixP diff = sq1.Subtract(sq2);
            diff.Print();

            // Тестируем прямоугольные матрицы
            Console.WriteLine("\n2. ПРЯМОУГОЛЬНЫЕ МАТРИЦЫ:");

            RectangularMatrix rect1 = new RectangularMatrix(2, 3);
            RectangularMatrix rect2 = new RectangularMatrix(3, 2);

            rect1.FillRandom(1, 5);
            rect2.FillRandom(1, 5);

            Console.WriteLine("Матрица C (2x3):");
            rect1.Print();

            Console.WriteLine("Матрица D (3x2):");
            rect2.Print();

            Console.WriteLine("C * D:");
            MatrixP product = rect1.Multiply(rect2);
            product.Print();

            Console.WriteLine("Умножение на число (2.5):");
            MatrixP scaled = rect1.Multiply(2.5);
            scaled.Print();

            // Тестируем индексатор
            Console.WriteLine("\n3. РАБОТА С ИНДЕКСАТОРОМ:");
            Console.WriteLine($"rect1[0,1] = {rect1[0, 1]:F2}");
            rect1[1, 2] = 99.99;
            Console.WriteLine("После rect1[1,2] = 99.99:");
            rect1.Print();
        }
        static void Main(string[] args)
        {
            //Console.WriteLine("Тестирование");

            //// Тест 1: Преобразование матрицы в массив и обратно
            //TestConversion();

            //// Тест 2: Проверка на некорректные данные
            //TestInvalidData();

            //Console.WriteLine("\nВсе тесты завершены!");
            Lab_2();
            Console.ReadKey();
        }

        static void TestConversion()
        {
            Console.WriteLine("\n1. Тест преобразования матрица -> массив -> матрица");

            // Создаем симметричную матрицу 3x3
            double[,] originalMatrix = {
                {1, 2, 3},
                {2, 4, 5},
                {3, 5, 6}
            };

            Console.WriteLine("\nИсходная матрица:");
            PrintMatrix(originalMatrix);

            try
            {
                // Преобразуем матрицу в массив
                double[] array = MatrixToArray.ConvertToArray(originalMatrix);

                Console.WriteLine("\nПолученный массив:");
                PrintArray(array);

                // Преобразуем массив обратно в матрицу
                double[,] restoredMatrix = ArrayToMatrix.ConvertToMatrix(array);

                Console.WriteLine("\nВосстановленная матрица:");
                PrintMatrix(restoredMatrix);

                // Проверяем, что матрицы идентичны
                if (AreMatricesEqual(originalMatrix, restoredMatrix))
                {
                    Console.WriteLine("\nПреобразование выполнено успешно! Матрицы идентичны.");
                }
                else
                {
                    Console.WriteLine("\nОшибка! Матрицы не идентичны.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nОшибка: {ex.Message}");
            }
        }

        static void TestInvalidData()
        {
            Console.WriteLine("\n2. Тест обработки некорректных данных");

            // Тест несимметричной матрицы
            double[,] nonSymmetricMatrix = {
                {1, 2, 3},
                {4, 5, 6},
                {7, 8, 9}
            };

            Console.WriteLine("\nТест несимметричной матрицы:");
            try
            {
                double[] array = MatrixToArray.ConvertToArray(nonSymmetricMatrix);
                Console.WriteLine("Ошибка! Должно было быть исключение для несимметричной матрицы.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Корректно обработано: {ex.Message}");
            }

            // Тест неквадратной матрицы
            double[,] nonSquareMatrix = {
                {1, 2, 3},
                {4, 5, 6}
            };

            Console.WriteLine("\nТест неквадратной матрицы:");
            try
            {
                double[] array = MatrixToArray.ConvertToArray(nonSquareMatrix);
                Console.WriteLine("Ошибка! Должно было быть исключение для неквадратной матрицы.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Корректно обработано: {ex.Message}");
            }

            // Тест некорректного массива
            double[] invalidArray = { 1, 2, 3, 4, 5 }; // Нельзя построить симметричную матрицу

            Console.WriteLine("\nТест некорректного массива:");
            try
            {
                double[,] matrix = ArrayToMatrix.ConvertToMatrix(invalidArray);
                Console.WriteLine("Ошибка! Должно было быть исключение для некорректного массива.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Корректно обработано: {ex.Message}");
            }
        }

        // Вспомогательные методы

        static void PrintMatrix(double[,] matrix)
        {
            int n = matrix.GetLength(0);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write($"{matrix[i, j],5:F1}");
                }
                Console.WriteLine();
            }
        }

        static void PrintArray(double[] array)
        {
            Console.Write("[");
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write($"{array[i]:F1}");
                if (i < array.Length - 1) Console.Write(", ");
            }
            Console.WriteLine("]");
        }

        static bool AreMatricesEqual(double[,] matrix1, double[,] matrix2)
        {
            // сравниваем столбцы и строки матриц
            if (matrix1.GetLength(0) != matrix2.GetLength(0) || matrix1.GetLength(1) != matrix2.GetLength(1)) 
            {
                return false; 
            }

            for (int i = 0; i < matrix1.GetLength(0); i++)
            {
                for (int j = 0; j < matrix1.GetLength(1); j++)
                {
                    if (matrix1[i, j] != matrix2[i, j])
                        return false;
                }
            }
            return true;
        }
    }
}