using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab_2.MatrixLibrary
{
    /// <summary>
    /// Абстрактный базовый класс для всех типов матриц
    /// Определяет общий интерфейс и базовую функциональность
    /// </summary>
    public abstract class Matrix
    {
        protected readonly double[,] data;
        protected readonly int rows;
        protected readonly int columns;

        /// <summary>
        /// Конструктор базового класса матрицы
        /// </summary>
        /// <param name="rows">Количество строк</param>
        /// <param name="columns">Количество столбцов</param>
        protected Matrix(int rows, int columns)
        {
            if (rows <= 0 || columns <= 0)
                throw new ArgumentException("Размеры матрицы должны быть положительными числами");

            this.rows = rows;
            this.columns = columns;
            this.data = new double[rows, columns];
        }

        /// <summary>
        /// Конструктор, создающий матрицу из двумерного массива
        /// </summary>
        /// <param name="data">Исходные данные матрицы</param>
        protected Matrix(double[,] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data), "Данные матрицы не могут быть null");

            this.data = (double[,])data.Clone();
            this.rows = data.GetLength(0);
            this.columns = data.GetLength(1);
        }

        // Базовые свойства
        public int Rows => rows;
        public int Columns => columns;
        public bool IsSquare => rows == columns;

        /// <summary>
        /// Индексатор для доступа к элементам матрицы
        /// </summary>
        public virtual double this[int row, int column]
        {
            get
            {
                ValidateIndices(row, column);
                return data[row, column];
            }
            set
            {
                ValidateIndices(row, column);
                data[row, column] = value;
            }
        }

        /// <summary>
        /// Проверка корректности индексов
        /// </summary>
        protected void ValidateIndices(int row, int column)
        {
            if (row < 0 || row >= rows)
                throw new IndexOutOfRangeException($"Индекс строки {row} выходит за границы [0, {rows - 1}]");

            if (column < 0 || column >= columns)
                throw new IndexOutOfRangeException($"Индекс столбца {column} выходит за границы [0, {columns - 1}]");
        }

        /// <summary>
        /// Проверка совместимости размеров матриц для операций
        /// </summary>
        protected void ValidateSameDimensions(Matrix other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), "Матрица не может быть null");

            if (rows != other.rows || columns != other.columns)
                throw new InvalidOperationException("Матрицы должны иметь одинаковые размеры");
        }

        /// <summary>
        /// Проверка совместимости для умножения матриц
        /// </summary>
        protected void ValidateMultiplicationCompatibility(Matrix other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), "Матрица не может быть null");

            if (columns != other.rows)
                throw new InvalidOperationException(
                    $"Количество столбцов первой матрицы ({columns}) должно совпадать с количеством строк второй матрицы ({other.rows})");
        }

        // Абстрактные методы - должны быть реализованы в производных классах
        public abstract Matrix Clone();
        public abstract double Determinant();
        public abstract Matrix Transpose();
        public abstract Matrix Inverse();

        // Виртуальные методы - могут быть переопределены в производных классах
        public virtual bool IsSymmetric()
        {
            if (!IsSquare) return false;

            for (int i = 0; i < rows; i++)
                for (int j = i + 1; j < columns; j++)
                    if (Math.Abs(data[i, j] - data[j, i]) > 1e-10)
                        return false;

            return true;
        }

        // Математические операции
        public virtual Matrix Add(Matrix other)
        {
            ValidateSameDimensions(other);
            var result = Clone();

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    result[i, j] += other[i, j];

            return result;
        }

        public virtual Matrix Subtract(Matrix other)
        {
            ValidateSameDimensions(other);
            var result = Clone();

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    result[i, j] -= other[i, j];

            return result;
        }

        public virtual Matrix Multiply(double scalar)
        {
            var result = Clone();

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    result[i, j] *= scalar;

            return result;
        }

        public virtual Matrix Multiply(Matrix other)
        {
            ValidateMultiplicationCompatibility(other);

            // Создаем матрицу подходящего типа для результата
            Matrix result = CreateResultMatrix(rows, other.columns);

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < other.columns; j++)
                    for (int k = 0; k < columns; k++)
                        result[i, j] += this[i, k] * other[k, j];

            return result;
        }

        /// <summary>
        /// Фабричный метод для создания матрицы результата
        /// Может быть переопределен в производных классах
        /// </summary>
        protected virtual Matrix CreateResultMatrix(int resultRows, int resultColumns)
        {
            // По умолчанию создаем прямоугольную матрицу
            return new RectangularMatrix(resultRows, resultColumns);
        }

        // Перегрузка операторов
        public static Matrix operator +(Matrix a, Matrix b) => a.Add(b);
        public static Matrix operator -(Matrix a, Matrix b) => a.Subtract(b);
        public static Matrix operator *(Matrix a, Matrix b) => a.Multiply(b);
        public static Matrix operator *(Matrix a, double scalar) => a.Multiply(scalar);
        public static Matrix operator *(double scalar, Matrix a) => a.Multiply(scalar);

        /// <summary>
        /// Строковое представление матрицы
        /// </summary>
        public override string ToString()
        {
            var lines = new List<string>();

            for (int i = 0; i < rows; i++)
            {
                var elements = new List<string>();
                for (int j = 0; j < columns; j++)
                    elements.Add(data[i, j].ToString("F2"));

                lines.Add($"[{string.Join(", ", elements)}]");
            }

            return string.Join(Environment.NewLine, lines);
        }

        /// <summary>
        /// Проверка равенства матриц с заданной точностью
        /// </summary>
        public virtual bool Equals(Matrix other, double tolerance = 1e-10)
        {
            if (other == null || rows != other.rows || columns != other.columns)
                return false;

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    if (Math.Abs(this[i, j] - other[i, j]) > tolerance)
                        return false;

            return true;
        }
    }
}