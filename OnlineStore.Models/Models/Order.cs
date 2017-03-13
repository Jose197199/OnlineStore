using Newtonsoft.Json;

namespace OnlineStore.Models
{
    [JsonObject]
    public class Order
    {
        public string ItemName { get; set; }
        public int Quantity { get; set; }
    }
}