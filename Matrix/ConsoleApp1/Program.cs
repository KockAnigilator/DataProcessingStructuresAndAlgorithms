using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main()
        {
            int[] array = { 10, 7, 8, 9, 1, 5 };
            QuickSort(array, 0, array.Length - 1);
            Console.WriteLine("Отсортированный массив:");
            Console.WriteLine(string.Join(", ", array));
        }

        static void QuickSort(int[] arr, int low, int high)
        {
            if (low < high)
            {
                int pivot = Partition(arr, low, high);
                QuickSort(arr, low, pivot - 1);
                QuickSort(arr, pivot + 1, high);
            }
        }

        static int Partition(int[] arr, int low, int high)
        {
            int pivot = arr[high];
            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                if (arr[j] <= pivot)
                {
                    i++;
                    // Прямой обмен элементов вместо Swap
                    int temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;
                }
            }

            // Поменять местами pivot и элемент после i
            int temp2 = arr[i + 1];
            arr[i + 1] = arr[high];
            arr[high] = temp2;

            return i + 1;
        }
    }
}
