using QueueLearning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Lab_6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Приоритетная очередь
            var pq = new PriorityQueue<int>();
            pq.Enqueue(3);
            pq.Enqueue(1);
            pq.Enqueue(2);
            Console.WriteLine(pq.Dequeue()); // 1
            Console.WriteLine(pq.Dequeue()); // 2

            // Кольцевая очередь
            var cq = new CircularQueue<int>(3);
            cq.Enqueue(1);
            cq.Enqueue(2);
            cq.Enqueue(3);
            Console.WriteLine(cq.Dequeue()); // 1
            cq.Enqueue(4); // Успешно добавит, т.к. освободилось место
            Console.WriteLine(cq.Dequeue()); // 2
        }
    }
}
