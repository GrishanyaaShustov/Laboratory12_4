using System;
using System.Collections;
using System.Collections.Generic;

// Узел списка
public class Point<T>
{
    public T Data { get; set; }
    public Point<T> Next { get; set; }

    public Point(T data)
    {
        Data = data;
        Next = null;
    }

    public override string ToString() => Data?.ToString();
    public override int GetHashCode() => Data?.GetHashCode() ?? 0;
}

// Обобщённая хеш-таблица с методом цепочек
public class MyHashTable<T>
{
    private Point<T>[] table;
    private int size, count;
    private const int defaultLength = 10;

    private readonly Func<T, object> keySelector;
    private readonly Func<object, object, bool> keyComparer;

    public MyHashTable(Func<T, object> keySelector, Func<object, object, bool> keyComparer = null)
    {
        this.keySelector = keySelector ?? throw new ArgumentNullException(nameof(keySelector));
        this.keyComparer = keyComparer ?? ((a, b) => a?.Equals(b) ?? b == null);

        size = defaultLength;
        table = new Point<T>[size];
        count = 0;
    }

    public int Count => count;
    public bool IsReadOnly => false; // Таблица изменяемая

    public int DefaultLength => defaultLength;
    public Point<T>[] Table => table;

    private int GetIndexByKey(object key)
        => Math.Abs(key?.GetHashCode() ?? 0) % size;

    private int GetIndexByItem(T item)
    {
        var key = keySelector(item);
        return GetIndexByKey(key);
    }

    // Добавление элемента
    public void Add(T item)
    {
        // Создаём новый узел с данными
        var node = new Point<T>(item);
        // Вычисляем индекс в таблице по ключу элемента
        int index = GetIndexByItem(item);

        // Если в этом индексе ещё нет элементов, просто ставим туда новый узел
        if (table[index] == null)
            table[index] = node;
        else
        {
            // Иначе идём по цепочке (связному списку) до конца и добавляем новый узел туда
            var current = table[index];
            while (current.Next != null) current = current.Next;
            current.Next = node;
        }

        // Увеличиваем счётчик элементов
        count++;
    }

    // Поиск элемента по ключу
    public T Find(object key)
    {
        // Вычисляем индекс по ключу, чтобы быстро перейти к нужной цепочке
        int index = GetIndexByKey(key);
        var current = table[index];

        // Перебираем элементы цепочки, сравнивая ключи
        while (current != null)
        {
            var dataKey = keySelector(current.Data);
            if (keyComparer(dataKey, key))
                return current.Data; // Найден элемент с нужным ключом

            current = current.Next;
        }

        // Если элемент не найден, возвращаем значение по умолчанию
        return default;
    }

    // Удаление по ключу
    public bool Remove(object key)
    {
        // Вычисляем индекс по ключу, чтобы найти нужную цепочку
        int index = GetIndexByKey(key);
        var current = table[index];
        Point<T> prev = null;

        // Перебираем цепочку, чтобы найти элемент для удаления
        while (current != null)
        {
            var dataKey = keySelector(current.Data);
            if (keyComparer(dataKey, key))
            {
                // Если элемент найден, корректируем ссылки, чтобы удалить его из цепочки
                if (prev == null)
                    table[index] = current.Next; // Удаляем первый элемент в цепочке
                else
                    prev.Next = current.Next; // Удаляем элемент в середине или конце

                count--;
                return true; // Успешное удаление
            }

            prev = current;
            current = current.Next;
        }

        // Элемент с таким ключом не найден
        return false;
    }

    // Очистка таблицы
    public void Clear()
    {
        table = new Point<T>[size];
        count = 0;
    }
}
