using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matrix.Lab_3
{
    public class Task_3
    {
        // Классическая сортировка вставками
        public void ClassicInsertionSort(int[] arr)
        {
            int n = arr.Length;
            for (int i = 1; i < n; i++)
            {
                int key = arr[i];
                int j = i - 1;

                // Линейный поиск позиции и сдвиг элементов
                while (j >= 0 && arr[j] > key)
                {
                    arr[j + 1] = arr[j];
                    j--;
                }
                arr[j + 1] = key;
            }
        }

        // Модифицированная сортировка вставками с бинарным поиском
        public void BinaryInsertionSort(int[] arr)
        {
            int n = arr.Length;
            for (int i = 1; i < n; i++)
            {
                int key = arr[i];
                int left = 0;
                int right = i - 1;

                // Бинарный поиск позиции для вставки
                while (left <= right)
                {
                    int mid = left + (right - left) / 2;
                    if (key < arr[mid])
                        right = mid - 1;
                    else
                        left = mid + 1;
                }

                // Сдвиг элементов
                for (int j = i - 1; j >= left; j--)
                {
                    arr[j + 1] = arr[j];
                }
                arr[left] = key;
            }
        }

        // Генерация случайного массива
        public int[] GenerateRandomArray(int size)
        {
            Random random = new Random();
            return Enumerable.Range(0, size)
                             .Select(_ => random.Next(-1000, 1000))
                             .ToArray();
        }

        // Копирование массива
        public int[] CopyArray(int[] arr)
        {
            int[] copy = new int[arr.Length];
            Array.Copy(arr, copy, arr.Length);
            return copy;
        }

    }
}
