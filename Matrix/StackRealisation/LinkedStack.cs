using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StackRealisation
{
    public class LinkedStack<T>
    {
        private class Node
        {
            public Node(T data)
            {
                Data = data;
                Next = null;
            }

            public T Data { get; set; }
            public Node Next { get; set; }
        }

        private Node top;
        private int count;

        public int Count => count;
        public bool IsEmpty => count == 0;

        public void Push(T item)
        {
            Node newNode = new Node(item);
            newNode.Next = top;
            top = newNode;
            count++;
        }

        public T Pop()
        {
            if (IsEmpty) throw new InvalidOperationException("Стек пуст");

            T data = top.Data;
            top = top.Next;
            count--;
            return data;
        }

        public T Peek()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Stack is empty");

            return top.Data;
        }

        public void Clear()
        {
            top = null;
            count = 0;
        }
    }
}
