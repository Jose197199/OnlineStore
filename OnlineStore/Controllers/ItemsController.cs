using OnlineStore.Models;
using OnlineStore.Repositories;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Web.Http;

namespace OnlineStore.Controllers
{
    public class ItemsController : ApiController
    {
        [HttpGet]
        public IEnumerable<Item> GetAllItems()
        {
            var itemRepo = new ItemRepository();
            return itemRepo.GetAll();
        }

        [HttpPost]
        public void Post(Item item)
        {
            // Note: this operation was added just for testing, therefore it requires special Admin token to be accessed
            AuthenticationHeaderValue authorizationHeader = Request.Headers.Authorization;
            if (authorizationHeader == null || authorizationHeader.Scheme != "Token" || authorizationHeader.Parameter != "C5F6FE0C-B7DA-4824-BB7B- D95F7CDA25A")
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            var itemRepo = new ItemRepository();
            itemRepo.Add(item);
        }

        [HttpDelete]
        public void Delete()
        {
            // Note: this operation was added just for testing, therefore it requires special Admin token to be accessed
            AuthenticationHeaderValue authorizationHeader = Request.Headers.Authorization;
            if (authorizationHeader == null || authorizationHeader.Scheme != "Token" || authorizationHeader.Parameter != "C5F6FE0C-B7DA-4824-BB7B- D95F7CDA25A")
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            var itemRepo = new ItemRepository();
            itemRepo.DeleteAll();
        }
    }
}
