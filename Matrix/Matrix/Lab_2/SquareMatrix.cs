using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matrix.Lab_2
{
    public class SquareMatrix : MatrixP
    {
        public int Size { get; private set; }

        public SquareMatrix(int size) : base(size, size)
        {
            Size = size;
        }

        public override MatrixP Add(MatrixP other)
        {
            // Проверяем, что other тоже квадратная матрица
            if (!(other is SquareMatrix))
                throw new ArgumentException("Можно складывать только с квадратной матрицей");

            SquareMatrix squareOther = (SquareMatrix)other;

            // Проверяем одинаковый размер
            if (Size != squareOther.Size)
                throw new ArgumentException("Матрицы должны быть одинакового размера");

            SquareMatrix result = new SquareMatrix(Size);

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    result[i, j] = this[i, j] + squareOther[i, j];
                }
            }

            return result;
        }

        public override MatrixP Subtract(MatrixP other)
        {
            if (!(other is SquareMatrix))
                throw new ArgumentException("Можно вычитать только квадратную матрицу");

            SquareMatrix squareOther = (SquareMatrix)other;

            if (Size != squareOther.Size)
                throw new ArgumentException("Матрицы должны быть одинакового размера");

            SquareMatrix result = new SquareMatrix(Size);

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    result[i, j] = this[i, j] - squareOther[i, j];
                }
            }

            return result;
        }

        public override MatrixP Multiply(MatrixP other)
        {
            if (other is SquareMatrix)
            {
                SquareMatrix squareOther = (SquareMatrix)other;

                // Умножение двух квадратных матриц
                if (Size != squareOther.Size)
                    throw new ArgumentException("Матрицы должны быть одинакового размера");

                SquareMatrix result = new SquareMatrix(Size);

                for (int i = 0; i < Size; i++)
                {
                    for (int j = 0; j < Size; j++)
                    {
                        result[i, j] = 0;
                        for (int k = 0; k < Size; k++)
                        {
                            result[i, j] += this[i, k] * squareOther[k, j];
                        }
                    }
                }

                return result;
            }
            else
            {
                // Умножение на прямоугольную матрицу
                if (Size != other.Rows)
                    throw new ArgumentException("Количество столбцов не совпадает с количеством строк второй матрицы");

                MatrixP result = new RectangularMatrix(Size, other.Columns);

                for (int i = 0; i < Size; i++)
                {
                    for (int j = 0; j < other.Columns; j++)
                    {
                        result[i, j] = 0;
                        for (int k = 0; k < Size; k++)
                        {
                            result[i, j] += this[i, k] * other[k, j];
                        }
                    }
                }

                return result;
            }
        }

        public override MatrixP Multiply(double scalar)
        {
            SquareMatrix result = new SquareMatrix(Size);

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    result[i, j] = this[i, j] * scalar;
                }
            }

            return result;
        }
    }
}
