using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueLearning
{
    public class PriorityQueue<T> where T : IComparable<T>
    {
        private List<T> _heap;

        public PriorityQueue()
        {
            _heap = new List<T>();
        }

        public int Count => _heap.Count;

        public void Enqueue(T item)
        {
            _heap.Add(item);
            int i = _heap.Count - 1;
            while (i > 0)
            {
                int parent = (i - 1) / 2;
                if (_heap[parent].CompareTo(_heap[i]) <= 0)
                    break;
                Swap(parent, i);
                i = parent;
            }
        }

        public T Dequeue()
        {
            if (_heap.Count == 0)
                throw new InvalidOperationException("Queue is empty");

            T result = _heap[0];
            _heap[0] = _heap[_heap.Count - 1];
            _heap.RemoveAt(_heap.Count - 1);

            int i = 0;
            while (true)
            {
                int left = 2 * i + 1;
                int right = 2 * i + 2;
                int smallest = i;

                if (left < _heap.Count &&
                    _heap[left].CompareTo(_heap[smallest]) < 0)
                    smallest = left;

                if (right < _heap.Count &&
                    _heap[right].CompareTo(_heap[smallest]) < 0)
                    smallest = right;

                if (smallest == i) break;
                Swap(i, smallest);
                i = smallest;
            }

            return result;
        }

        public T Peek()
        {
            if (_heap.Count == 0)
                throw new InvalidOperationException("Queue is empty");
            return _heap[0];
        }

        private void Swap(int i, int j)
        {
            T temp = _heap[i];
            _heap[i] = _heap[j];
            _heap[j] = temp;
        }
    }
}
