using System;
using Car;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class MyHashTableTests
    {
        private MyHashTable<Car.Car> table;

        [SetUp]
        public void Setup()
        {
            // Указываем селектор ключа и компаратор
            table = new MyHashTable<Car.Car>(
                car => car.CarId.Number, // keySelector
                (a, b) => (int)a == (int)b // keyComparer
            );
        }

        private static LightCar CreateCar(int id)
        {
            var car = new LightCar();
            car.RandomInit();
            car.CarId = new IdNumber(id);
            return car;
        }

        private class CollidingId : IdNumber
        {
            public CollidingId(int number) : base(number) { }
            public override int GetHashCode() => 42; // Заставляем коллизии
        }

        private static LightCar CreateCollidingCar(int id)
        {
            var car = new LightCar();
            car.RandomInit();
            car.CarId = new CollidingId(id);
            return car;
        }

        [Test]
        public void DefaultLength_Is10()
        {
            Assert.That(table.DefaultLength, Is.EqualTo(10));
        }

        [Test]
        public void Table_HasLengthEqualToDefaultLength()
        {
            Assert.That(table.Table, Is.Not.Null);
            Assert.That(table.Table.Length, Is.EqualTo(table.DefaultLength));
        }

        [Test]
        public void Add_IncreasesCount()
        {
            table.Add(CreateCar(1));
            Assert.That(table.Count, Is.EqualTo(1));
        }

        [Test]
        public void Add_HandlesCollisions()
        {
            table.Add(CreateCollidingCar(1));
            table.Add(CreateCollidingCar(2));
            Assert.That(table.Count, Is.EqualTo(2));
        }

        [Test]
        public void Remove_ExistingElement_DecreasesCount()
        {
            var car = CreateCar(10);
            table.Add(car);
            var result = table.Remove(10);
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True);
                Assert.That(table.Count, Is.EqualTo(0));
            });
        }

        [Test]
        public void Remove_NonExistingElement_ReturnsFalse()
        {
            table.Add(CreateCar(11));
            var result = table.Remove(999);
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.False);
                Assert.That(table.Count, Is.EqualTo(1));
            });
        }

        [Test]
        public void Find_ReturnsElement_WhenExists()
        {
            var car = CreateCar(20);
            table.Add(car);
            var found = table.Find(20);
            Assert.That(found, Is.SameAs(car));
        }

        [Test]
        public void Find_ReturnsNull_WhenNotExists()
        {
            table.Add(CreateCar(21));
            var found = table.Find(22);
            Assert.That(found, Is.Null);
        }

        [Test]
        public void Clear_EmptiesTable()
        {
            table.Add(CreateCar(40));
            table.Add(CreateCar(41));
            table.Clear();
            Assert.Multiple(() =>
            {
                Assert.That(table.Count, Is.EqualTo(0));
                Assert.That(table.Find(40), Is.Null);
            });
        }

        [Test]
        public void Add_ElementCanBeFoundAfterAdd()
        {
            var car = CreateCar(100);
            table.Add(car);
            var found = table.Find(100);
            Assert.That(found, Is.SameAs(car));
        }
    }
}
