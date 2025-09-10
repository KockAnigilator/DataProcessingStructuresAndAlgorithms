using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matrix
{
    public class ArrayToMatrix
    {
        /// <summary>
        /// Преобразует одномерный массив в симметричную матрицу
        /// </summary>
        /// <param name="array">Массив с элементами нижнего треугольника</param>
        /// <returns>Симметричная матрица</returns>
        public static double[,] ConvertToMatrix(double[] array)
        {
            // Находим размер матрицы из уравнения n(n+1)/2 = array.Length
            int n = (int)((-1 + Math.Sqrt(1 + 8 * array.Length)) / 2);

            if (n * (n + 1) / 2 != array.Length)
            {
                throw new ArgumentException("Некорректный размер массива для симметричной матрицы");
            }

            double[,] matrix = new double[n, n];
            int index = 0;

            // Заполняем матрицу
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    matrix[i, j] = array[index];
                    matrix[j, i] = array[index]; // Симметричный элемент
                    index++;
                }
            }

            return matrix;
        }
    }
}
