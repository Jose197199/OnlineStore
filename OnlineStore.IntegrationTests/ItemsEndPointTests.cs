using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnlineStore.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace OnlineStore.IntegrationTests
{
    [TestClass]
    public class ItemsEndPointTests
    {
        private static ItemsEndPoint mItemsEndPoint;

        [ClassInitialize]
        public static void SuiteSetup(TestContext context)
        {
            mItemsEndPoint = new ItemsEndPoint(ConfigurationManager.AppSettings["serviceUri"]);
        }

        [TestCleanup]
        public void TestTearDown()
        {
            mItemsEndPoint.DeleteAllItems().GetAwaiter().GetResult();
        }

        [TestMethod]
        public async Task EmptyListOfItems()
        {
            IList<Item> items = await mItemsEndPoint.GetAllItems();
            Assert.AreEqual(0, items.Count);
        }

        [TestMethod]
        public async Task AddItemReadItBack()
        {
            await mItemsEndPoint.AddItem(new Item { Name = "Binoculars", Description = "Black binoculars with, 10x magnification.", Price = 50, Stock = 2 });
            IList<Item> items = await mItemsEndPoint.GetAllItems();
            Assert.AreEqual(1, items.Count);
            Assert.AreEqual("Binoculars", items[0].Name);
        }

        [TestMethod]
        public async Task AddMultipleItemsReadThemBack()
        {
            await mItemsEndPoint.AddItem(new Item { Name = "Binoculars", Description = "Black binoculars with, 10x magnification.", Price = 50, Stock = 2 });
            await mItemsEndPoint.AddItem(new Item { Name = "Refracting Telescope", Description = "Refractor telescope, 80x magnification.", Price = 150, Stock = 5 });
            await mItemsEndPoint.AddItem(new Item { Name = "Reflecting Telescope", Description = "Reflecting telescope, 200x magnification.", Price = 1000, Stock = 10 });
            IList<Item> items = await mItemsEndPoint.GetAllItems();
            Assert.AreEqual(3, items.Count);
            Assert.AreEqual("Binoculars", items[0].Name);
            Assert.AreEqual("Refracting Telescope", items[1].Name);
            Assert.AreEqual("Reflecting Telescope", items[2].Name);
        }
    }
}
