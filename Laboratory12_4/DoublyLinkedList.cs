using System;
using System.Collections;
using System.Collections.Generic;

public class DoublyLinkedList<T> : ICloneable where T : ICloneable, IComparable<T>
{
    public class Node
    {
        public T Data;
        public Node Prev, Next;

        public Node(T data)
        {
            Data = data;
        }
    }

    public Node head;
    public Node tail;
    public int Count { get; private set; }

    // Добавление элемента в конец
    public void Add(T data)
    {
        Node newNode = new Node(data);
        if (head == null)
            head = tail = newNode;
        else
        {
            tail.Next = newNode;
            newNode.Prev = tail;
            tail = newNode;
        }
        Count++;
    }

    // Добавление элементов на нечётные позиции с помощью фабричной функции генерации
    public void AddOddGenerated(int count, Func<T> generator)
    {
        if (generator == null) throw new ArgumentNullException(nameof(generator));

        for (int i = 0; i < count; i++)
        {
            if ((Count + 1) % 2 == 1)  // Нечётная позиция (позиции считаем с 1)
            {
                Add(generator());
            }
            else
            {
                Add(default); // Добавляем null (default для ссылочных типов)
            }
        }
    }

    // Универсальное удаление элементов начиная с первого, у которого ключ совпадает, и до конца списка
    // keySelector -  из T извлекает ключ для сравнения
    public void DeleteFromKey(IComparable key, Func<T, IComparable> keySelector)
    {
        if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
        if (key == null) throw new ArgumentNullException(nameof(key));

        Node current = head;
        while (current != null)
        {
            // Если ключ совпал — удаляем current и все последующие
            if (current.Data != null && keySelector(current.Data).CompareTo(key) == 0)
            {
                // Открепляем хвост с текущего узла
                if (current.Prev != null)
                {
                    current.Prev.Next = null;
                    tail = current.Prev;
                }
                else
                {
                    // Если совпадение в первом элементе — список очищается полностью
                    head = tail = null;
                }

                // Освобождаем ссылки на узлы и уменьшаем счетчик
                while (current != null)
                {
                    Node next = current.Next;
                    current.Prev = current.Next = null;
                    Count--;
                    current = next;
                }
                break; // удаление завершено
            }
            current = current.Next;
        }
    }

    // Глубокое клонирование списка
    public object Clone()
    {
        var cloneList = new DoublyLinkedList<T>();
        Node current = head;

        while (current != null)
        {
            if (current.Data != null)
                cloneList.Add((T)current.Data.Clone());
            else
                cloneList.Add(default);

            current = current.Next;
        }

        return cloneList;
    }

    // Очистка списка
    public void Clear()
    {
        Node current = head;
        while (current != null)
        {
            Node next = current.Next;
            current.Prev = current.Next = null;
            current = next;
        }
        head = tail = null;
        Count = 0;
    }
    
}
