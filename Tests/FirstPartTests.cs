using System;
using System.Collections.Generic;
using NUnit.Framework;
using Car;

namespace Tests
{
    [TestFixture]
    public class TestsFirstPart
    {
        private DoublyLinkedList<Car.Car> list;

        [SetUp]
        public void Setup()
        {
            list = new DoublyLinkedList<Car.Car>();
        }

        [Test]
        public void Add_ShouldIncreaseCount_WhenElementAdded()
        {
            var car = new LightCar();
            car.RandomInit();
            list.Add(car);
            Assert.AreEqual(1, list.Count);
        }

        [Test]
        public void AddOddGenerated_ShouldAddCorrectNumberOfItems()
        {
            int toAdd = 5;
            list.AddOddGenerated(toAdd, () =>
            {
                var c = new LightCar();
                c.RandomInit();
                return c;
            });
            Assert.AreEqual(toAdd, list.Count);
        }

        [Test]
        public void Clone_ShouldCreateDeepCopy()
        {
            var car = new BigCar();
            car.RandomInit();
            list.Add(car);

            var clone = (DoublyLinkedList<Car.Car>)list.Clone();

            Assert.AreEqual(list.Count, clone.Count);
            Assert.AreNotSame(list, clone);
            Assert.AreNotSame(list.head.Data, clone.head.Data);
            Assert.AreEqual(list.head.Data.Brand, clone.head.Data.Brand);
        }

        [Test]
        public void DeleteFromName_ShouldRemoveAllAfterFirstMatch()
        {
            var car1 = new LightCar();
            car1.RandomInit();
            car1.Brand = "DeleteMe";

            var car2 = new BigCar();
            car2.RandomInit();

            var car3 = new DeliveryCar();
            car3.RandomInit();

            list.Add(car1);
            list.Add(car2);
            list.Add(car3);

            list.DeleteFromKey("DeleteMe", c => c.Brand);

            Assert.AreEqual(0, list.Count);
        }

        [Test]
        public void Clear_ShouldEmptyList()
        {
            var car = new LightCar
            {
                Brand = "Test",
                Year = 2020,
                Color = "Black",
                Price = 10000,
                Clearance = 150,
                CarId = new IdNumber(1)
            };
            list.Add(car);
            list.Clear();

            Assert.AreEqual(0, list.Count);
            Assert.IsNull(list.head);
            Assert.IsNull(list.tail);
        }

        [Test]
        public void DeleteFromName_ShouldDoNothing_WhenBrandNotFound()
        {
            var car1 = new LightCar();
            car1.RandomInit();
            car1.Brand = "Brand1";

            var car2 = new LightCar();
            car2.RandomInit();
            car2.Brand = "Brand2";

            list.Add(car1);
            list.Add(car2);

            list.DeleteFromKey("Nonexistent", c => c.Brand);

            Assert.AreEqual(2, list.Count);
        }
    }
}
