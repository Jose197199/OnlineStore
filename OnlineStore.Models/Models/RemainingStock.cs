using Newtonsoft.Json;

namespace OnlineStore.Models.Models
{
    [JsonObject]
    public class RemainingStock
    {
        public string ItemName { get; set; }
        public int Stock { get; set; }
    }
}
