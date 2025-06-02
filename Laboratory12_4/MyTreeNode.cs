using System;
using System.Collections;
using System.Collections.Generic;

public class Tree<T> where T : ICloneable, IComparable<T>
{
    private readonly Func<T, int> _keySelector;
    
    public T Data;
    public Tree<T> Left;
    public Tree<T> Right;

    public Tree(T data, Func<T, int> keySelector)
    {
        Data = data;
        _keySelector = keySelector ?? throw new ArgumentNullException(nameof(keySelector));
    }
    
    public Tree(T data)
    {
        Data = data;
    }

    // Построение идеально сбалансированного дерева из отсортированного списка
    public static Tree<T> BuildBalancedTree(List<T> items)
    {
        items.Sort(); // Сортируем элементы
        return BuildBalancedRecursive(items, 0, items.Count - 1);
    }

    // Рекурсивное построение сбалансированного дерева по отсортированному списку
    private static Tree<T> BuildBalancedRecursive(List<T> items, int left, int right)
    {
        if (left > right) return null;
        int mid = (left + right) / 2;
        var node = new Tree<T>((T)items[mid].Clone()); // Середина — корень поддерева
        node.Left = BuildBalancedRecursive(items, left, mid - 1);  // Левое поддерево
        node.Right = BuildBalancedRecursive(items, mid + 1, right); // Правое поддерево
        return node;
    }

    // Построение сбалансированного дерева поиска из дерева
    public static Tree<T> BuildBalancedSearchTree(Tree<T> root)
    {
        if (root == null) return null;

        // Получаем список элементов в порядке возрастания
        List<T> items = root.ToListInOrder();

        // Убираем дубликаты
        List<T> uniqueItems = new List<T>();
        T lastItem = default;
        bool isFirst = true;

        foreach (T item in items)
        {
            if (isFirst || item.CompareTo(lastItem) != 0)
            {
                uniqueItems.Add((T)item.Clone());
                lastItem = item;
                isFirst = false;
            }
        }

        // Строим новое сбалансированное дерево
        return BuildBalancedTree(uniqueItems);
    }

    // Вывод дерева на экран в виде текста
    public void ShowTree(Func<T, string> format, string indent = "", bool isRight = false)
    {
        if (Right != null)
        {
            Right.ShowTree(format, indent + (isRight ? "        " : "│       "), true);
        }

        Console.Write(indent);

        if (isRight)
            Console.Write("┌─────");
        else if (indent.Length > 0)
            Console.Write("└─────");

        Console.WriteLine(format(Data));

        if (Left != null)
        {
            Left.ShowTree(format, indent + (isRight ? "│       " : "        "), false);
        }
    }

    // Обход дерева в симметричном порядке (слева-корень-справа)
    public List<T> ToListInOrder()
    {
        var list = new List<T>();
        InOrder(this, list);
        return list;
    }

    private void InOrder(Tree<T> node, List<T> list)
    {
        if (node == null) return;
        InOrder(node.Left, list);
        list.Add(node.Data);
        InOrder(node.Right, list);
    }

    // Поиск максимального элемента по заданному ключу
    public T FindMaxElement(Func<T, int> keySelector)
    {
        if (Data == null) return default;
        T max = Data;
        FindMaxRecursive(this, ref max, keySelector);
        return max;
    }

    private void FindMaxRecursive(Tree<T> node, ref T max, Func<T, int> keySelector)
    {
        if (node == null) return;
        if (keySelector(node.Data) > keySelector(max))
            max = node.Data;
        FindMaxRecursive(node.Left, ref max, keySelector);
        FindMaxRecursive(node.Right, ref max, keySelector);
    }

    // Удаление элемента по ключу
    public static Tree<T> RemoveByKey(Tree<T> root, int key, Func<T, int> keySelector)
    {
        if (root == null) return null;

        int currentKey = keySelector(root.Data);

        if (key < currentKey)
            root.Left = RemoveByKey(root.Left, key, keySelector); // идём влево
        else if (key > currentKey)
            root.Right = RemoveByKey(root.Right, key, keySelector); // идём вправо
        else
        {
            // Найден элемент
            if (root.Left == null) return root.Right;
            if (root.Right == null) return root.Left;

            // У узла есть два потомка: ищем минимум в правом поддереве
            Tree<T> min = root.Right;
            while (min.Left != null) min = min.Left;

            root.Data = (T)min.Data.Clone(); // заменяем данные
            root.Right = RemoveByKey(root.Right, keySelector(min.Data), keySelector); // удаляем дубль
        }

        return root;
    }
}