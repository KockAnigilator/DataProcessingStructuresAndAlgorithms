using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matrix.Lab_2
{
    public class RectangularMatrix : MatrixP
    {
        public RectangularMatrix(int rows, int columns) : base(rows, columns)
        {

        }

        public override MatrixP Add(MatrixP other)
        {
            // Проверяем одинаковые размеры
            if (Rows != other.Rows || Columns != other.Columns)
                throw new ArgumentException("Матрицы должны иметь одинаковые размеры");

            RectangularMatrix result = new RectangularMatrix(Rows, Columns);

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    result[i, j] = this[i, j] + other[i, j];
                }
            }

            return result;
        }

        public override MatrixP Subtract(MatrixP other)
        {
            if (Rows != other.Rows || Columns != other.Columns)
                throw new ArgumentException("Матрицы должны иметь одинаковые размеры");

            RectangularMatrix result = new RectangularMatrix(Rows, Columns);

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    result[i, j] = this[i, j] - other[i, j];
                }
            }

            return result;
        }

        public override MatrixP Multiply(MatrixP other)
        {
            // Проверяем совместимость для умножения
            if (Columns != other.Rows)
                throw new ArgumentException("Количество столбцов первой матрицы должно равняться количеству строк второй матрицы");

            RectangularMatrix result = new RectangularMatrix(Rows, other.Columns);

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < other.Columns; j++)
                {
                    result[i, j] = 0;
                    for (int k = 0; k < Columns; k++)
                    {
                        result[i, j] += this[i, k] * other[k, j];
                    }
                }
            }

            return result;
        }

        public override MatrixP Multiply(double scalar)
        {
            RectangularMatrix result = new RectangularMatrix(Rows, Columns);

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    result[i, j] = this[i, j] * scalar;
                }
            }

            return result;
        }



    }
}
