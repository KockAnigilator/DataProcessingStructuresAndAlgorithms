using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matrix.Lab_2
{
    public abstract class MatrixP
    {
        public double[,] data;

        protected MatrixP(int rows, int columns)
        {
            if (rows <= 0 || columns <= 0)
                throw new ArgumentException("Размеры матрицы должны быть положительными числами");

            Rows = rows;
            Columns = columns;
            data = new double[rows, columns];
        }

        public int Rows {  get; set; }
        public int Columns { get; set; }

        public double this[int row, int column]
        {
            get
            {
                if (row < 0 || row >= Rows)
                    throw new IndexOutOfRangeException($"Строка {row} вне диапазона");
                if (column < 0 || column >= Columns)
                    throw new IndexOutOfRangeException($"Столбец {column} вне диапазона");

                return data[row, column];
            }
            set
            {
                if (row < 0 || row >= Rows)
                    throw new IndexOutOfRangeException($"Строка {row} вне диапазона");
                if (column < 0 || column >= Columns)
                    throw new IndexOutOfRangeException($"Столбец {column} вне диапазона");

                data[row, column] = value;
            }
        }

        // Метод для заполнения матрицы случайными числами
        public void FillRandom(double minValue = 0, double maxValue = 10)
        {
            Random random = new Random();

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    data[i, j] = Math.Round(minValue + (maxValue - minValue) * random.NextDouble(), 2);
                }
            }
        }

        // Метод для распечатки матрицы
        public void Print()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Console.Write($"{data[i, j]:F2}\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        // Абстрактные методы для операций
        public abstract MatrixP Add(MatrixP other);
        public abstract MatrixP Subtract(MatrixP other);
        public abstract MatrixP Multiply(MatrixP other);
        public abstract MatrixP Multiply(double scalar);

    }
}
