using System;
using NUnit.Framework;
using Car;
using System.Linq;

namespace Tests
{
    [TestFixture]
    public class MyCollectionTests
    {
        private MyCollection<Car.Car> collection;

        [SetUp]
        public void Setup()
        {
            collection = new MyCollection<Car.Car>();
        }

        private static LightCar CreateCar(int id)
        {
            var car = new LightCar();
            car.RandomInit();
            car.CarId = new IdNumber(id);
            return car;
        }

        [Test]
        public void DefaultConstructor_CreatesEmptyCollection()
        {
            Assert.That(collection.Count, Is.EqualTo(0));
        }

        [Test]
        public void ConstructorWithLength_CreatesSpecifiedNumberOfElements()
        {
            var c = new MyCollection<Car.Car>(5, Car => Car.CarId.Number);
            Assert.That(c.Count, Is.EqualTo(5));
        }

        [Test]
        public void CopyConstructor_CopiesElements()
        {
            var original = new MyCollection<Car.Car>(3, Car => Car.CarId.Number);
            var copy = new MyCollection<Car.Car>(original);

            Assert.Multiple(() =>
            {
                Assert.That(copy.Count, Is.EqualTo(3));
                Assert.That(copy.ToList(), Is.EquivalentTo(original.ToList()));
            });
        }

        [Test]
        public void Add_IncreasesCount()
        {
            var car = CreateCar(1);
            ((ICollection<Car.Car>)collection).Add(car);
            Assert.That(collection.Count, Is.EqualTo(1));
        }

        [Test]
        public void Contains_ReturnsTrueForAddedItem()
        {
            var car = CreateCar(2);
            collection.Add(car);
            Assert.That(collection.Contains(car), Is.True);
        }

        [Test]
        public void Remove_RemovesItemAndDecreasesCount()
        {
            var car = CreateCar(3);
            collection.Add(car);
            var removed = collection.Remove(car);

            Assert.Multiple(() =>
            {
                Assert.That(removed, Is.True);
                Assert.That(collection.Contains(car), Is.False);
                Assert.That(collection.Count, Is.EqualTo(0));
            });
        }

        [Test]
        public void CopyTo_CopiesElementsCorrectly()
        {
            var car1 = CreateCar(4);
            var car2 = CreateCar(5);
            collection.Add(car1);
            collection.Add(car2);

            var array = new Car.Car[2];
            collection.CopyTo(array, 0);

            Assert.That(array, Does.Contain(car1).And.Contain(car2));
        }

        [Test]
        public void GetEnumerator_IteratesAllElements()
        {
            var car1 = CreateCar(6);
            var car2 = CreateCar(7);
            collection.Add(car1);
            collection.Add(car2);

            var list = collection.ToList();
            Assert.That(list.Count, Is.EqualTo(2));
            Assert.That(list, Does.Contain(car1).And.Contain(car2));
        }
    }
}
