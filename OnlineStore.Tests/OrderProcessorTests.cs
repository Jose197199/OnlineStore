using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnlineStore.Business;
using OnlineStore.Models;
using OnlineStore.Repositories;

namespace OnlineStore.Tests
{
    [TestClass]
    public class OrderProcessorTests
    {
        [TestInitialize]
        public void Setup()
        {
            var itemRepo = new ItemRepository();
            itemRepo.Add(new Item { Name = "Binoculars", Description = "Black binoculars with, 10x magnification.", Price = 50, Stock = 2 });
            itemRepo.Add(new Item { Name = "Refracting Telescope", Description = "Refractor telescope, 80x magnification.", Price = 150, Stock = 5 });
            itemRepo.Add(new Item { Name = "Reflecting Telescope", Description = "Reflecting telescope, 200x magnification.", Price = 1000, Stock = 10 });
        }

        [TestCleanup]
        public void TearDown()
        {
            var itemRepo = new ItemRepository();
            itemRepo.DeleteAll();
        }

        [TestMethod]
        public void OrderZeroQuantity()
        {
            var orderProcessor = new OrderProcessor();
            OrderResult result = orderProcessor.OrderItem("Refracting Telescope", 0);
            Assert.AreEqual(OrderStatus.InvalidQuantity, result.Status);
        }

        [TestMethod]
        public void OrderNegativeQuantity()
        {
            var orderProcessor = new OrderProcessor();
            OrderResult result = orderProcessor.OrderItem("Refracting Telescope", -5);
            Assert.AreEqual(OrderStatus.InvalidQuantity, result.Status);
        }

        [TestMethod]
        public void OrderUnexistentProduct()
        {
            var orderProcessor = new OrderProcessor();
            OrderResult result = orderProcessor.OrderItem("Spy Glass", 1);
            Assert.AreEqual(OrderStatus.UnexistentProduct, result.Status);
        }

        [TestMethod]
        public void OrderOutOfStockProduct()
        {
            var orderProcessor = new OrderProcessor();
            OrderResult result = orderProcessor.OrderItem("Binoculars", 3);
            Assert.AreEqual(OrderStatus.OutOfStockProduct, result.Status);
        }

        [TestMethod]
        public void OrderInStockQuantity()
        {
            var orderProcessor = new OrderProcessor();
            OrderResult result = orderProcessor.OrderItem("Refracting Telescope", 2);
            Assert.AreEqual(OrderStatus.Processed, result.Status);
            Assert.AreEqual(3, result.RemainingStock);
        }

        [TestMethod]
        public void OrderMultipleTimeSameItem()
        {
            var orderProcessor = new OrderProcessor();
            orderProcessor.OrderItem("Binoculars", 1);
            OrderResult result = orderProcessor.OrderItem("Binoculars", 1);
            Assert.AreEqual(OrderStatus.Processed, result.Status);
            Assert.AreEqual(0, result.RemainingStock);
        }

        [TestMethod]
        public void OrderMultipleTimeSameItemTillOutOfStock()
        {
            var orderProcessor = new OrderProcessor();
            orderProcessor.OrderItem("Binoculars", 1);
            orderProcessor.OrderItem("Binoculars", 1);
            OrderResult result = orderProcessor.OrderItem("Binoculars", 1);
            Assert.AreEqual(OrderStatus.OutOfStockProduct, result.Status);
        }

        [TestMethod]
        public void OrderMultipleProducts()
        {
            var orderProcessor = new OrderProcessor();
            OrderResult result1 = orderProcessor.OrderItem("Binoculars", 1);
            OrderResult result2 = orderProcessor.OrderItem("Refracting Telescope", 2);
            OrderResult result3 = orderProcessor.OrderItem("Reflecting Telescope", 7);
            Assert.AreEqual(OrderStatus.Processed, result1.Status);
            Assert.AreEqual(1, result1.RemainingStock);
            Assert.AreEqual(OrderStatus.Processed, result2.Status);
            Assert.AreEqual(3, result2.RemainingStock);
            Assert.AreEqual(OrderStatus.Processed, result3.Status);
            Assert.AreEqual(3, result3.RemainingStock);
        }
    }
}
