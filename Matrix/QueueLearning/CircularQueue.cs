using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueLearning
{
    public class CircularQueue<T>
    {
        private T[] _elements;
        private int _front;
        private int _rear;
        private int _count;
        private int _capacity;

        public CircularQueue(int capacity)
        {
            _capacity = capacity;
            _elements = new T[capacity];
            _front = 0;
            _rear = -1;
            _count = 0;
        }

        public int Count => _count;

        public void Enqueue(T item)
        {
            if (_count == _capacity)
                throw new InvalidOperationException("Queue is full");

            _rear = (_rear + 1) % _capacity;
            _elements[_rear] = item;
            _count++;
        }

        public T Dequeue()
        {
            if (_count == 0)
                throw new InvalidOperationException("Queue is empty");

            T item = _elements[_front];
            _front = (_front + 1) % _capacity;
            _count--;
            return item;
        }

        public T Peek()
        {
            if (_count == 0)
                throw new InvalidOperationException("Queue is empty");
            return _elements[_front];
        }

        public bool IsEmpty => _count == 0;
        public bool IsFull => _count == _capacity;
    }
}
