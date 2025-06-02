using System;
using System.Collections;
using System.Collections.Generic;

// Обобщённая коллекция на основе хеш-таблицы
public class MyCollection<T> : MyHashTable<T>, ICollection<T> where T : IInit, new()
{
    public MyCollection() 
        : base(item => item?.GetHashCode()) // Ключ по умолчанию — хеш объекта
    {
    }

    public MyCollection(int length)
        : base(item => item?.GetHashCode())
    {
        for (int i = 0; i < length; i++)
        {
            T element = new T();
            element.RandomInit();
            Add(element);
        }
    }

    public MyCollection(MyCollection<T> c)
        : base(item => item?.GetHashCode())
    {
        foreach (T item in c)
        {
            Add(item);
        }
    }

    public bool IsReadOnly => false;

    // Реализация Add из ICollection<T>
    void ICollection<T>.Add(T item) => Add(item);

    // Реализация Contains (использует Find)
    public bool Contains(T item)
    {
        var key = item?.GetHashCode();
        var found = Find(key);
        return found != null && found.Equals(item);
    }

    // Копирование элементов в массив
    public void CopyTo(T[] array, int arrayIndex)
    {
        foreach (T item in this)
        {
            if (arrayIndex >= array.Length)
                throw new ArgumentException("Массив мал");

            array[arrayIndex++] = item;
        }
    }

    // Удаление элемента по значению
    public bool Remove(T item)
    {
        var key = item?.GetHashCode();
        return base.Remove(key);
    }

    // Реализация перечислителя
    public IEnumerator<T> GetEnumerator()
    {
        foreach (var head in Table)
        {
            var current = head;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
