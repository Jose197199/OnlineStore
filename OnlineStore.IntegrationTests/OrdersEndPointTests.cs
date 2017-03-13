using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnlineStore.Models;
using OnlineStore.Models.Models;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace OnlineStore.IntegrationTests
{
    [TestClass]
    public class OrdersEndPointTests
    {
        private static HttpClient mHttpClient;
        private static ItemsEndPoint mItemsEndPoint;

        [ClassInitialize]
        public static void SuiteSetup(TestContext context)
        {
            string serviceUri = ConfigurationManager.AppSettings["serviceUri"];
            mHttpClient = new HttpClient { BaseAddress = new Uri(serviceUri) };
            mItemsEndPoint = new ItemsEndPoint(serviceUri);
            PopulateInventory().GetAwaiter().GetResult();
        }

        [ClassCleanup]
        public static void SuiteTearDown()
        {
            ClearInventory().GetAwaiter().GetResult();
        }

        [TestInitialize]
        public void Testinitialize()
        {
            mHttpClient.DefaultRequestHeaders.Clear();
        }

        [TestMethod]
        public async Task UnauthenticatedAccess()
        {
            HttpResponseMessage response = await OrderItem(new Order { ItemName = "Binoculars", Quantity = 1 });
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task OrderInvalidQuantity()
        {
            AddAuthenticationToken();
            HttpResponseMessage response = await OrderItem(new Order { ItemName = "Binoculars", Quantity = -1 });
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task OrderUnexistentItem()
        {
            AddAuthenticationToken();
            HttpResponseMessage response = await OrderItem(new Order { ItemName = "Spy Glass", Quantity = 1 });
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task OrderValidItem()
        {
            AddAuthenticationToken();
            HttpResponseMessage response = await OrderItem(new Order { ItemName = "Binoculars", Quantity = 1 });
            RemainingStock remainingStock = await response.Content.ReadAsAsync<RemainingStock>();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(1, remainingStock.Stock);
        }

        private async Task<HttpResponseMessage> OrderItem(Order order)
        {
            HttpResponseMessage response = await mHttpClient.PostAsJsonAsync("api/orders", order);
            return response;
        }

        private void AddAuthenticationToken()
        {
            mHttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Token", "55B19D7B-31E9-4CB5-AF0B-EAAC8FE6422A");
        }

        private static async Task PopulateInventory()
        {
            await mItemsEndPoint.AddItem(new Item { Name = "Binoculars", Description = "Black binoculars with, 10x magnification.", Price = 50, Stock = 2 });
            await mItemsEndPoint.AddItem(new Item { Name = "Refracting Telescope", Description = "Refractor telescope, 80x magnification.", Price = 150, Stock = 5 });
            await mItemsEndPoint.AddItem(new Item { Name = "Reflecting Telescope", Description = "Reflecting telescope, 200x magnification.", Price = 1000, Stock = 10 });
        }

        private static async Task ClearInventory()
        {
            await mItemsEndPoint.DeleteAllItems();
        }
    }
}
