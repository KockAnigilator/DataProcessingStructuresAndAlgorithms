using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackRealisation
{
    public class ArrayStack<T>
    {
        private T[] items;
        private int top;
        private int capacity;

        public int Count => top + 1;
        public bool IsEmpty => top == -1;

        public ArrayStack(int initialCapacity = 10)
        {
            capacity = initialCapacity;
            items = new T[capacity];
            top = -1;
        }

        public void Push(T item)
        {
            if (top == capacity - 1)
                Resize(capacity * 2);

            items[++top] = item;
        }

        public T Pop()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Stack is empty");

            T item = items[top--];

            if (top > 0 && top == capacity / 4)
                Resize(capacity / 2);

            return item;
        }

        public T Peek()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Stack is empty");

            return items[top];
        }

        public void Clear()
        {
            Array.Clear(items, 0, Count);
            top = -1;
            capacity = 10;
            Resize(capacity);
        }

        private void Resize(int newCapacity)
        {
            T[] newArray = new T[newCapacity];
            Array.Copy(items, newArray, top + 1);
            items = newArray;
            capacity = newCapacity;
        }
    }
}
