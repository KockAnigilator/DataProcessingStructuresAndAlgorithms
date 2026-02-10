using System;
using System.Linq;

namespace Lab_2.MatrixLibrary
{
    /// <summary>
    /// Класс для работы с диагональными матрицами
    /// Оптимизирует хранение и операции для диагональных матриц
    /// </summary>
    public class DiagonalMatrix : SquareMatrix
    {
        private readonly double[] diagonal;

        /// <summary>
        /// Создает диагональную матрицу из массива диагональных элементов
        /// </summary>
        public DiagonalMatrix(double[] diagonal) : base(diagonal.Length)
        {
            this.diagonal = (double[])diagonal.Clone();

            // Заполняем основную диагональ
            for (int i = 0; i < diagonal.Length; i++)
                data[i, i] = diagonal[i];
        }

        /// <summary>
        /// Оптимизированный индексатор для диагональной матрицы
        /// </summary>
        public override double this[int row, int column]
        {
            get
            {
                ValidateIndices(row, column);
                return row == column ? diagonal[row] : 0.0;
            }
            set
            {
                ValidateIndices(row, column);

                if (row == column)
                {
                    diagonal[row] = value;
                    data[row, column] = value;
                }
                else if (Math.Abs(value) > 1e-10)
                {
                    throw new InvalidOperationException(
                        "Нельзя установить ненулевое значение вне диагонали в диагональной матрице");
                }
            }
        }

        /// <summary>
        /// Оптимизированное вычисление определителя (произведение диагональных элементов)
        /// </summary>
        public override double Determinant()
        {
            return diagonal.Aggregate(1.0, (current, element) => current * element);
        }

        /// <summary>
        /// Транспонирование диагональной матрицы - возвращает саму себя
        /// </summary>
        public override Matrix Transpose()
        {
            return Clone(); // Диагональная матрица симметрична
        }

        /// <summary>
        /// Оптимизированное вычисление обратной матрицы
        /// </summary>
        public override Matrix Inverse()
        {
            var invertedDiagonal = new double[diagonal.Length];

            for (int i = 0; i < diagonal.Length; i++)
            {
                if (Math.Abs(diagonal[i]) < 1e-10)
                    throw new InvalidOperationException("Матрица вырождена, обратной матрицы не существует");

                invertedDiagonal[i] = 1.0 / diagonal[i];
            }

            return new DiagonalMatrix(invertedDiagonal);
        }

        /// <summary>
        /// Оптимизированное сложение диагональных матриц
        /// </summary>
        public override Matrix Add(Matrix other)
        {
            if (other is DiagonalMatrix diagonalOther)
            {
                ValidateSameDimensions(diagonalOther);
                var resultDiagonal = new double[diagonal.Length];

                for (int i = 0; i < diagonal.Length; i++)
                    resultDiagonal[i] = diagonal[i] + diagonalOther.diagonal[i];

                return new DiagonalMatrix(resultDiagonal);
            }

            return base.Add(other);
        }

        /// <summary>
        /// Оптимизированное умножение диагональных матриц
        /// </summary>
        public override Matrix Multiply(Matrix other)
        {
            if (other is DiagonalMatrix diagonalOther)
            {
                ValidateMultiplicationCompatibility(diagonalOther);
                var resultDiagonal = new double[diagonal.Length];

                for (int i = 0; i < diagonal.Length; i++)
                    resultDiagonal[i] = diagonal[i] * diagonalOther.diagonal[i];

                return new DiagonalMatrix(resultDiagonal);
            }

            return base.Multiply(other);
        }

        /// <summary>
        /// Создает глубокую копию матрицы
        /// </summary>
        public override Matrix Clone()
        {
            return new DiagonalMatrix((double[])diagonal.Clone());
        }

        /// <summary>
        /// Проверка является ли матрица единичной
        /// </summary>
        public bool IsIdentity(double tolerance = 1e-10)
        {
            return diagonal.All(element => Math.Abs(element - 1.0) < tolerance);
        }
    }
}