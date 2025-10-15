using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Lab_4
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Тестирование реализации стека");
            Console.WriteLine("=============================");

            while (true)
            {
                Console.WriteLine("\nВыберите реализацию стека:");
                Console.WriteLine("1 - Стек на связном списке");
                Console.WriteLine("2 - Стек на массиве");
                Console.WriteLine("3 - Выход");
                Console.Write("Ваш выбор: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        TestLinkedStack();
                        break;
                    case "2":
                        TestArrayStack();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор!");
                        break;
                }
            }
        }

        static void TestLinkedStack()
        {
            Console.WriteLine("\n=== Тестирование стека на связном списке ===");
            LinkedStack<int> stack = new LinkedStack<int>();
            TestStack(stack);
        }

        static void TestArrayStack()
        {
            Console.WriteLine("\n=== Тестирование стека на массиве ===");
            Console.Write("Введите начальную емкость (по умолчанию 10): ");
            string input = Console.ReadLine();

            ArrayStack<int> stack;
            if (int.TryParse(input, out int capacity) && capacity > 0)
            {
                stack = new ArrayStack<int>(capacity);
                Console.WriteLine($"Создан стек с емкостью {capacity}");
            }
            else
            {
                stack = new ArrayStack<int>();
                Console.WriteLine("Создан стек с емкостью по умолчанию (10)");
            }

            TestStack(stack);
        }

        static void TestStack<T>(IStack<T> stack) where T : IConvertible
        {
            while (true)
            {
                Console.WriteLine($"\nТекущее состояние стека:");
                Console.WriteLine($"Количество элементов: {stack.Count}");
                Console.WriteLine($"Пустой: {stack.IsEmpty}");

                Console.WriteLine("\nДоступные операции:");
                Console.WriteLine("1 - Push (добавить элемент)");
                Console.WriteLine("2 - Pop (извлечь элемент)");
                Console.WriteLine("3 - Peek (посмотреть верхний элемент)");
                Console.WriteLine("4 - Clear (очистить стек)");
                Console.WriteLine("5 - Назад к выбору реализации");
                Console.Write("Ваш выбор: ");

                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            Console.Write("Введите значение: ");
                            string input = Console.ReadLine();

                            // Пробуем преобразовать в нужный тип
                            T value = (T)Convert.ChangeType(input, typeof(T));
                            stack.Push(value);
                            Console.WriteLine($"Добавлен элемент: {value}");
                            break;

                        case "2":
                            if (!stack.IsEmpty)
                            {
                                T popped = stack.Pop();
                                Console.WriteLine($"Извлечен элемент: {popped}");
                            }
                            else
                            {
                                Console.WriteLine("Стек пуст!");
                            }
                            break;

                        case "3":
                            if (!stack.IsEmpty)
                            {
                                T peeked = stack.Peek();
                                Console.WriteLine($"Верхний элемент: {peeked}");
                            }
                            else
                            {
                                Console.WriteLine("Стек пуст!");
                            }
                            break;

                        case "4":
                            stack.Clear();
                            Console.WriteLine("Стек очищен");
                            break;

                        case "5":
                            return;

                        default:
                            Console.WriteLine("Неверный выбор!");
                            break;
                    }
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
                catch (FormatException)
                {
                    Console.WriteLine("Ошибка: неверный формат ввода!");
                }
                catch (OverflowException)
                {
                    Console.WriteLine("Ошибка: число слишком большое!");
                }
            }
        }
    }

    // Интерфейс для унификации тестирования
    public interface IStack<T>
    {
        void Push(T item);
        T Pop();
        T Peek();
        void Clear();
        int Count { get; }
        bool IsEmpty { get; }
    }

    // Классы стеков (те же, что и в предыдущем ответе)
    public class LinkedStack<T> : IStack<T>
    {
        private class Node
        {
            public T Data { get; set; }
            public Node Next { get; set; }

            public Node(T data)
            {
                Data = data;
                Next = null;
            }
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
            if (IsEmpty)
                throw new InvalidOperationException("Stack is empty");

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

    public class ArrayStack<T> : IStack<T>
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

            // Уменьшаем массив если он заполнен меньше чем на 25%
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
