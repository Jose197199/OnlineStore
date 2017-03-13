using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnlineStore.Models;
using OnlineStore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineStore.Tests
{
    [TestClass]
    public class ItemRepositoryTests
    {
        [TestCleanup]
        public void TearDown()
        {
            var itemRepo = new ItemRepository();
            itemRepo.DeleteAll();
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException), "Invalid item.")]
        public void AddNullItemToRepository()
        {
            var itemRepo = new ItemRepository();
            itemRepo.Add(null);
        }

        [TestMethod]
        public void AddItemToRepository()
        {
            var itemRepo = new ItemRepository();
            itemRepo.Add(new Item { Name = "Binoculars", Description = "Black binoculars with, 10x magnification.", Price = 50, Stock = 2 });
            var items = itemRepo.GetAll().ToList();
            Assert.AreEqual(1, items.Count);
            Assert.AreEqual("Binoculars", items[0].Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException), "Duplicate item.")]
        public void AddDuplicateItemToRepository()
        {
            var itemRepo = new ItemRepository();
            itemRepo.Add(new Item { Name = "Binoculars", Description = "Black binoculars with, 10x magnification.", Price = 50, Stock = 2 });
            itemRepo.Add(new Item { Name = "Binoculars", Description = "Black binoculars with, 10x magnification.", Price = 50, Stock = 2 });
        }

        [TestMethod]
        public void AddItemThenFindByName()
        {
            var itemRepo = new ItemRepository();
            itemRepo.Add(new Item { Name = "Binoculars", Description = "Black binoculars with, 10x magnification.", Price = 50, Stock = 2 });
            Item binoculars = itemRepo.FindByName("Binoculars");
            Assert.AreEqual("Binoculars", binoculars.Name);
            Assert.AreEqual(50, binoculars.Price);
            Assert.AreEqual(2, binoculars.Stock);
        }

        [TestMethod]
        public void UnexistentItem()
        {
            var itemRepo = new ItemRepository();
            itemRepo.Add(new Item { Name = "Binoculars", Description = "Black binoculars with, 10x magnification.", Price = 50, Stock = 2 });
            Item oversizedBinoculars = itemRepo.FindByName("Binoculars Oversized");
            Assert.IsNull(oversizedBinoculars);
        }

        [TestMethod]
        public void AddMultipleItemsToRepository()
        {
            var itemRepo = new ItemRepository();
            itemRepo.Add(new Item { Name = "Binoculars", Description = "Black binoculars with, 10x magnification.", Price = 50, Stock = 2 });
            itemRepo.Add(new Item { Name = "Refracting Telescope", Description = "Refractor telescope, 80x magnification.", Price = 150, Stock = 5 });
            itemRepo.Add(new Item { Name = "Reflecting Telescope", Description = "Reflecting telescope, 200x magnification.", Price = 1000, Stock = 10 });
            IList<Item> items = itemRepo.GetAll().ToList();
            Assert.AreEqual(3, items.Count);
            Assert.AreEqual("Binoculars", items[0].Name);
            Assert.AreEqual("Refracting Telescope", items[1].Name);
            Assert.AreEqual("Reflecting Telescope", items[2].Name);
        }
    }
}