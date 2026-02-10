using Lab_2.MatrixLibrary;
using System;

namespace Test_Lab_2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== ДЕМОНСТРАЦИЯ РАБОТЫ С МАТРИЦАМИ ===\n");

            // Создание и тестирование квадратных матриц
            TestSquareMatrices();

            // Создание и тестирование диагональных матриц
            TestDiagonalMatrices();

            // Тестирование операций
            TestOperations();

            Console.WriteLine("\nВсе тесты завершены успешно!");
        }

        static void TestSquareMatrices()
        {
            Console.WriteLine("1. ТЕСТИРОВАНИЕ КВАДРАТНЫХ МАТРИЦ:");

            // Создание матрицы 2x2
            var matrixA = new SquareMatrix(new double[,] {
                { 1, 2 },
                { 3, 4 }
            });

            Console.WriteLine("Матрица A:");
            Console.WriteLine(matrixA);
            Console.WriteLine($"Определитель: {matrixA.Determinant()}");
            Console.WriteLine($"След: {((SquareMatrix)matrixA).Trace()}");
            Console.WriteLine($"Симметрична: {matrixA.IsSymmetric()}\n");

            // Создание единичной матрицы
            var identity = SquareMatrix.CreateIdentity(2);
            Console.WriteLine("Единичная матрица:");
            Console.WriteLine(identity);

            // Транспонирование
            var transposed = matrixA.Transpose();
            Console.WriteLine("Транспонированная матрица A:");
            Console.WriteLine(transposed);
        }

        static void TestDiagonalMatrices()
        {
            Console.WriteLine("\n2. ТЕСТИРОВАНИЕ ДИАГОНАЛЬНЫХ МАТРИЦ:");

            // Создание диагональной матрицы
            var diagMatrix = new DiagonalMatrix(new double[] { 2, 3, 4 });

            Console.WriteLine("Диагональная матрица:");
            Console.WriteLine(diagMatrix);
            Console.WriteLine($"Определитель: {diagMatrix.Determinant()}\n");

            // Обратная диагональная матрица
            var inverseDiag = diagMatrix.Inverse();
            Console.WriteLine("Обратная диагональная матрица:");
            Console.WriteLine(inverseDiag);
        }

        static void TestOperations()
        {
            Console.WriteLine("\n3. ТЕСТИРОВАНИЕ ОПЕРАЦИЙ:");

            var matrix1 = new SquareMatrix(new double[,] {
                { 1, 2 },
                { 3, 4 }
            });

            var matrix2 = new SquareMatrix(new double[,] {
                { 5, 6 },
                { 7, 8 }
            });

            // Сложение
            var sum = matrix1 + matrix2;
            Console.WriteLine("Сложение матриц:");
            Console.WriteLine("A + B =");
            Console.WriteLine(sum);

            // Умножение
            var product = matrix1 * matrix2;
            Console.WriteLine("Умножение матриц:");
            Console.WriteLine("A * B =");
            Console.WriteLine(product);

            // Умножение на скаляр
            var scaled = matrix1 * 2.5;
            Console.WriteLine("Умножение на скаляр (A * 2.5):");
            Console.WriteLine(scaled);

            // Проверка равенства
            var clone = matrix1.Clone();
            Console.WriteLine($"Матрица A равна своей копии: {matrix1.Equals(clone)}");
        }
    }
}