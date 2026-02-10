using System;

namespace Lab_2.MatrixLibrary
{
    /// <summary>
    /// Класс для работы с прямоугольными матрицами произвольного размера
    /// </summary>
    public class RectangularMatrix : Matrix
    {
        /// <summary>
        /// Создает прямоугольную матрицу заданного размера
        /// </summary>
        /// <param name="rows">Количество строк</param>
        /// <param name="columns">Количество столбцов</param>
        public RectangularMatrix(int rows, int columns) : base(rows, columns) { }

        /// <summary>
        /// Создает прямоугольную матрицу из двумерного массива
        /// </summary>
        /// <param name="data">Данные матрицы</param>
        public RectangularMatrix(double[,] data) : base(data) { }

        /// <summary>
        /// Создает глубокую копию матрицы
        /// </summary>
        public override Matrix Clone()
        {
            return new RectangularMatrix((double[,])data.Clone());
        }

        /// <summary>
        /// Вычисляет определитель матрицы
        /// </summary>
        /// <exception cref="InvalidOperationException">Если матрица не квадратная</exception>
        public override double Determinant()
        {
            if (!IsSquare)
                throw new InvalidOperationException("Определитель можно вычислить только для квадратной матрицы");

            // Для прямоугольных матриц используем базовую реализацию через квадратную матрицу
            var squareMatrix = new SquareMatrix(data);
            return squareMatrix.Determinant();
        }

        /// <summary>
        /// Вычисляет транспонированную матрицу
        /// </summary>
        public override Matrix Transpose()
        {
            var transposed = new RectangularMatrix(columns, rows);

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    transposed[j, i] = this[i, j];

            return transposed;
        }

        /// <summary>
        /// Вычисляет обратную матрицу
        /// </summary>
        /// <exception cref="InvalidOperationException">Если матрица не квадратная</exception>
        public override Matrix Inverse()
        {
            if (!IsSquare)
                throw new InvalidOperationException("Обратную матрицу можно вычислить только для квадратной матрицы");

            // Для прямоугольных матриц используем базовую реализацию через квадратную матрицу
            var squareMatrix = new SquareMatrix(data);
            return squareMatrix.Inverse();
        }

        /// <summary>
        /// Переопределяем фабричный метод для создания результата того же типа
        /// </summary>
        protected override Matrix CreateResultMatrix(int resultRows, int resultColumns)
        {
            return new RectangularMatrix(resultRows, resultColumns);
        }
    }
}