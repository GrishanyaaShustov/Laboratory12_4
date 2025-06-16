using System;
using System.Collections;
using System.Collections.Generic;

// Обобщённая коллекция на основе хеш-таблицы
public class MyCollection<T> : MyHashTable<T>, ICollection<T> where T : IInit, new()
{
    public MyCollection() 
        : base(item => item?.GetHashCode())
    {
    }

    public MyCollection(int length, Func<T, object> keySelector, Func<object, object, bool> keyComparer = null)
        : base(keySelector, keyComparer)
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
            Add((T)((ICloneable)item).Clone());
        }
    }

    public bool IsReadOnly => false;

    // Реализация Contains (использует Find)
    public bool Contains(T item)
    {
        var key =  KeySelector(item);
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

            array[arrayIndex++] = (T)((ICloneable)item).Clone();
        }
    }

    // Удаление элемента по значению
    public new bool Remove(T item)
    {
        var key = KeySelector(item);
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