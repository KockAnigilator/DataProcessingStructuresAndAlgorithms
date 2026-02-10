using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_3.Tasks
{
    /// <summary>
    /// Решение задачи о двух шарах и n-этажном здании (1 задача)
    /// </summary>
    public class EggDroppingSolver
    {
        /// <summary>
        /// Находит минимальное количество попыток в худшем случае
        /// </summary>
        /// <param name="floors">Количество этажей</param>
        /// <returns>Минимальное количество попыток</returns>
        public static int FindMinimumAttempts(int floors)
        {
            if (floors <= 0) return 0;

            // Для 2 шаров используем оптимальную стратегию
            // Ищем минимальное k, такое что k*(k+1)/2 >= floors
            int k = 1;
            while (k * (k + 1) / 2 < floors)
            {
                k++;
            }
            return k;
        }

        /// <summary>
        /// Находит оптимальную последовательность бросков
        /// </summary>
        /// <param name="floors">Количество этажей</param>
        /// <returns>Список этажей для бросков</returns>
        public static List<int> FindOptimalSequence(int floors)
        {
            var sequence = new List<int>();
            int currentFloor = 0;
            int step = FindMinimumAttempts(floors);

            while (currentFloor < floors)
            {
                currentFloor += step;
                if (currentFloor > floors) currentFloor = floors;
                sequence.Add(currentFloor);
                step--;
                if (step <= 0) break;
            }

            return sequence;
        }

        /// <summary>
        /// Динамическое программирование для общего случая (любое количество шаров)
        /// </summary>
        public static int FindMinimumAttemptsDP(int floors, int eggs)
        {
            if (floors <= 1 || eggs == 1) return floors;

            int[,] dp = new int[eggs + 1, floors + 1];

            // Базовые случаи
            for (int i = 1; i <= eggs; i++)
            {
                dp[i, 1] = 1; // 1 этаж - 1 попытка
                dp[i, 0] = 0; // 0 этажей - 0 попыток
            }

            for (int j = 1; j <= floors; j++)
            {
                dp[1, j] = j; // 1 шар - проверяем все этажи
            }

            // Заполняем таблицу
            for (int i = 2; i <= eggs; i++)
            {
                for (int j = 2; j <= floors; j++)
                {
                    dp[i, j] = int.MaxValue;
                    for (int k = 1; k <= j; k++)
                    {
                        // Максимум из двух случаев: шар разбился или не разбился
                        int attempts = 1 + Math.Max(dp[i - 1, k - 1], dp[i, j - k]);
                        if (attempts < dp[i, j])
                        {
                            dp[i, j] = attempts;
                        }
                    }
                }
            }

            return dp[eggs, floors];
        }
    }
}
