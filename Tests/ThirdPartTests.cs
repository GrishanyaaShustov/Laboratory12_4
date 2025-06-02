using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class TreeTests
    {
        private class TestItem : IComparable<TestItem>, ICloneable
        {
            public int Value { get; set; }

            public TestItem(int value)
            {
                Value = value;
            }

            public int CompareTo(TestItem other)
            {
                return Value.CompareTo(other.Value);
            }

            public object Clone()
            {
                return new TestItem(Value);
            }

            public override string ToString() => Value.ToString();
        }
        
        private Tree<TestItem> BuildTree(params int[] values)
        {
            var items = GetSampleItems(values);
            return Tree<TestItem>.BuildBalancedTree(items);
        }

        private Func<TestItem, int> keySelector = i => i.Value;

        private List<TestItem> GetSampleItems(params int[] values)
        {
            var list = new List<TestItem>();
            foreach (var v in values)
                list.Add(new TestItem(v));
            return list;
        }

        [Test]
        public void BuildBalancedTree_CreatesBalancedTree()
        {
            var items = GetSampleItems(1, 2, 3, 4, 5, 6, 7);
            var tree = Tree<TestItem>.BuildBalancedTree(items);
            var inOrder = tree.ToListInOrder();
            Assert.That(inOrder.Count, Is.EqualTo(7));
            CollectionAssert.AreEqual(new[] {1, 2, 3, 4, 5, 6, 7}, inOrder.ConvertAll(i => i.Value));
        }

        [Test]
        public void ToListInOrder_ReturnsSortedList()
        {
            var items = GetSampleItems(3, 1, 4, 2);
            var tree = Tree<TestItem>.BuildBalancedTree(items);
            var inOrder = tree.ToListInOrder();
            CollectionAssert.AreEqual(new[] {1, 2, 3, 4}, inOrder.ConvertAll(i => i.Value));
        }

        [Test]
        public void FindMaxElement_ReturnsMaxByKey()
        {
            var items = GetSampleItems(10, 20, 30, 5);
            var tree = Tree<TestItem>.BuildBalancedTree(items);
            var max = tree.FindMaxElement(i => i.Value);
            Assert.That(max.Value, Is.EqualTo(30));
        }

        [Test]
        public void BuildBalancedSearchTree_RemovesDuplicates()
        {
            var items = GetSampleItems(1, 2, 2, 3, 3, 3, 4);
            var tree = Tree<TestItem>.BuildBalancedTree(items);
            var searchTree = Tree<TestItem>.BuildBalancedSearchTree(tree);
            var inOrder = searchTree.ToListInOrder();
            CollectionAssert.AreEqual(new[] {1, 2, 3, 4}, inOrder.ConvertAll(i => i.Value));
        }

        [Test]
        public void RemoveByKey_RemovesCorrectNode()
        {
            var items = GetSampleItems(5, 3, 7, 2, 4, 6, 8);
            var tree = Tree<TestItem>.BuildBalancedTree(items);
            tree = Tree<TestItem>.RemoveByKey(tree, 4, i => i.Value);
            var inOrder = tree.ToListInOrder();
            CollectionAssert.AreEqual(new[] {2, 3, 5, 6, 7, 8}, inOrder.ConvertAll(i => i.Value));
        }

        [Test]
        public void RemoveByKey_RemovesRootCorrectly()
        {
            var items = GetSampleItems(1, 2, 3);
            var tree = Tree<TestItem>.BuildBalancedTree(items);
            int originalRoot = tree.Data.Value;
            tree = Tree<TestItem>.RemoveByKey(tree, originalRoot, i => i.Value);
            var inOrder = tree.ToListInOrder();
            CollectionAssert.AreEqual(new[] {1, 2, 3}.Where(v => v != originalRoot).ToArray(), inOrder.ConvertAll(i => i.Value));
        }

        [Test]
        public void RemoveByKey_HandlesNotFound()
        {
            var items = GetSampleItems(1, 2, 3);
            var tree = Tree<TestItem>.BuildBalancedTree(items);
            var modified = Tree<TestItem>.RemoveByKey(tree, 999, i => i.Value);
            var inOrder = modified.ToListInOrder();
            CollectionAssert.AreEqual(new[] {1, 2, 3}, inOrder.ConvertAll(i => i.Value));
        }
    }
}
