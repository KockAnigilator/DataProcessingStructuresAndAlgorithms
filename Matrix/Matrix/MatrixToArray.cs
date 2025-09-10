using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matrix
{
    public class MatrixToArray
    {
        /// <summary>
        /// Преобразует симметричную матрицу в одномерный массив
        /// </summary>
        /// <param name="matrix">Симметричная матрица</param>
        /// <returns>Одномерный массив с элементами нижнего треугольника</returns>
        public static double[] ConvertToArray(double[,] matrix)
        {
            int n = matrix.GetLength(0);

            // Проверка на квадратность матрицы
            if (n != matrix.GetLength(1))
            {
                throw new ArgumentException("Матрица должна быть квадратной");
            }

            // Проверка на симметричность
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    if (matrix[i, j] != matrix[j, i])
                    {
                        throw new ArgumentException("Матрица не является симметричной");
                    }
                }
            }

            // Размер массива для нижнего треугольника
            int arraySize = n * (n + 1) / 2;
            double[] result = new double[arraySize];
            int index = 0;

            // Заполняем массив элементами нижнего треугольника (включая диагональ)
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    result[index++] = matrix[i, j];
                }
            }

            return result;
        }
    }
}