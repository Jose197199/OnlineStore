using OnlineStore.Business;
using OnlineStore.Models;
using OnlineStore.Models.Models;
using System.Net;
using System.Net.Http.Headers;
using System.Web.Http;

namespace OnlineStore.Controllers
{
    public class OrdersController : ApiController
    {
        [HttpPost]
        public RemainingStock Post([FromBody]Order order)
        {
            AuthenticationHeaderValue authorizationHeader = Request.Headers.Authorization;
            if (authorizationHeader == null || authorizationHeader.Scheme != "Token" || authorizationHeader.Parameter != "55B19D7B-31E9-4CB5-AF0B-EAAC8FE6422A")
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            var orderProcessor = new OrderProcessor();
            OrderResult orderResult = orderProcessor.OrderItem(order.ItemName, order.Quantity);
            switch (orderResult.Status)
            {
                case OrderStatus.InvalidQuantity:
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                case OrderStatus.UnexistentProduct:
                case OrderStatus.OutOfStockProduct:
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                case OrderStatus.UnexpectedFailure:
                    throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }

            return new RemainingStock() { ItemName = order.ItemName, Stock = orderResult.RemainingStock };
        }
    }
}
