using Newtonsoft.Json;

namespace OnlineStore.Models
{
    [JsonObject]
    public class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }

        public Item Clone()
        {
            return new Item { Name = this.Name, Description = this.Description, Price = this.Price, Stock = this.Stock };
        }
    }
}