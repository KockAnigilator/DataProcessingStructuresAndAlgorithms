using System;

namespace Lab_2.MatrixLibrary
{
    /// <summary>
    /// Класс для работы с квадратными матрицами
    /// Наследует базовую функциональность и добавляет специфические операции
    /// </summary>
    public class SquareMatrix : Matrix
    {
        /// <summary>
        /// Создает квадратную матрицу заданного размера
        /// </summary>
        /// <param name="size">Размер матрицы (количество строк и столбцов)</param>
        public SquareMatrix(int size) : base(size, size) { }

        /// <summary>
        /// Создает квадратную матрицу из двумерного массива
        /// </summary>
        /// <param name="data">Данные матрицы</param>
        /// <exception cref="ArgumentException">Если массив не квадратный</exception>
        public SquareMatrix(double[,] data) : base(data)
        {
            if (!IsSquare)
                throw new ArgumentException("Квадратная матрица должна иметь одинаковое количество строк и столбцов");
        }

        /// <summary>
        /// Создает единичную матрицу заданного размера
        /// </summary>
        public static SquareMatrix CreateIdentity(int size)
        {
            var identity = new SquareMatrix(size);

            for (int i = 0; i < size; i++)
                identity[i, i] = 1.0;

            return identity;
        }

        /// <summary>
        /// Создает диагональную матрицу из массива элементов
        /// </summary>
        public static SquareMatrix CreateDiagonal(double[] diagonal)
        {
            var matrix = new SquareMatrix(diagonal.Length);

            for (int i = 0; i < diagonal.Length; i++)
                matrix[i, i] = diagonal[i];

            return matrix;
        }

        /// <summary>
        /// Вычисляет определитель матрицы методом Гаусса
        /// </summary>
        public override double Determinant()
        {
            return CalculateDeterminant((double[,])data.Clone());
        }

        /// <summary>
        /// Рекурсивный расчет определителя методом разложения по строке
        /// </summary>
        private double CalculateDeterminant(double[,] matrix)
        {
            int size = matrix.GetLength(0);

            // Базовый случай: матрица 1x1
            if (size == 1) return matrix[0, 0];

            // Базовый случай: матрица 2x2
            if (size == 2)
                return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];

            double determinant = 0;
            int sign = 1;

            // Разложение по первой строке
            for (int j = 0; j < size; j++)
            {
                // Пропускаем нулевые элементы для оптимизации
                if (Math.Abs(matrix[0, j]) > 1e-10)
                {
                    var minor = GetMinor(matrix, 0, j);
                    determinant += sign * matrix[0, j] * CalculateDeterminant(minor);
                }
                sign = -sign;
            }

            return determinant;
        }

        /// <summary>
        /// Получает минор матрицы (удаляет указанные строку и столбец)
        /// </summary>
        private double[,] GetMinor(double[,] matrix, int rowToRemove, int colToRemove)
        {
            int size = matrix.GetLength(0);
            var minor = new double[size - 1, size - 1];

            int minorRow = 0;
            for (int i = 0; i < size; i++)
            {
                if (i == rowToRemove) continue;

                int minorCol = 0;
                for (int j = 0; j < size; j++)
                {
                    if (j == colToRemove) continue;

                    minor[minorRow, minorCol] = matrix[i, j];
                    minorCol++;
                }
                minorRow++;
            }

            return minor;
        }

        /// <summary>
        /// Вычисляет транспонированную матрицу
        /// </summary>
        public override Matrix Transpose()
        {
            var transposed = new SquareMatrix(rows);

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < rows; j++)
                    transposed[i, j] = this[j, i];

            return transposed;
        }

        /// <summary>
        /// Вычисляет обратную матрицу методом алгебраических дополнений
        /// </summary>
        public override Matrix Inverse()
        {
            double det = Determinant();

            if (Math.Abs(det) < 1e-10)
                throw new InvalidOperationException("Матрица вырождена, обратной матрицы не существует");

            var inverse = new SquareMatrix(rows);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    var minor = GetMinor(data, i, j);
                    double cofactor = CalculateDeterminant(minor) * Math.Pow(-1, i + j);
                    inverse[j, i] = cofactor / det; // Транспонирование сразу
                }
            }

            return inverse;
        }

        /// <summary>
        /// Создает глубокую копию матрицы
        /// </summary>
        public override Matrix Clone()
        {
            return new SquareMatrix((double[,])data.Clone());
        }

        /// <summary>
        /// Переопределяем фабричный метод для создания результата того же типа
        /// </summary>
        protected override Matrix CreateResultMatrix(int resultRows, int resultColumns)
        {
            // Если результат умножения квадратных матриц - квадратная матрица
            if (resultRows == resultColumns)
                return new SquareMatrix(resultRows);
            else
                return new RectangularMatrix(resultRows, resultColumns);
        }

        /// <summary>
        /// Вычисляет след матрицы (сумму диагональных элементов)
        /// </summary>
        public double Trace()
        {
            double trace = 0;

            for (int i = 0; i < rows; i++)
                trace += this[i, i];

            return trace;
        }
    }
}