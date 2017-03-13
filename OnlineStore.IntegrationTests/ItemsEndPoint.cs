using OnlineStore.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace OnlineStore.IntegrationTests
{
    public class ItemsEndPoint
    {
        private HttpClient mHttpClient;

        public ItemsEndPoint(string baseUrl)
        {
            mHttpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
        }

        public async Task AddItem(Item item)
        {
            AddAuthenticationToken();
            HttpResponseMessage response = await mHttpClient.PostAsJsonAsync("api/items", item);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAllItems()
        {
            AddAuthenticationToken();
            HttpResponseMessage response = await mHttpClient.DeleteAsync("api/items");
            response.EnsureSuccessStatusCode();
        }

        public async Task<IList<Item>> GetAllItems()
        {
            HttpResponseMessage response = await mHttpClient.GetAsync("/api/items");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IList<Item>>();
            }
            else
                throw new ApplicationException(string.Format("Unxpected HTTP Error: {0}", response.StatusCode));
        }

        private void AddAuthenticationToken()
        {
            mHttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Token", "C5F6FE0C-B7DA-4824-BB7B- D95F7CDA25A");
        }
    }
}
