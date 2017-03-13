using OnlineStore.Models;
using OnlineStore.Repositories;

namespace OnlineStore.Business
{
    public enum OrderStatus
    {
        Processed,
        UnexistentProduct,
        OutOfStockProduct,
        InvalidQuantity,
        UnexpectedFailure
    }

    public class OrderResult
    {
        public OrderStatus Status;
        public int RemainingStock { get; set; }
    }

    public class OrderProcessor
    {
        public OrderResult OrderItem(string name, int quantity)
        {
            ItemRepository itemRepo = new ItemRepository();
            Item item = itemRepo.FindByName(name);
            if (quantity <= 0)
                return new OrderResult { Status = OrderStatus.InvalidQuantity, RemainingStock = 0 };
            if (item == null)
                return new OrderResult { Status = OrderStatus.UnexistentProduct, RemainingStock = 0 };
            if (item.Stock < quantity)
                return new OrderResult { Status = OrderStatus.OutOfStockProduct, RemainingStock = item.Stock };

            item.Stock -= quantity;
            OrderRepository orderRepo = new OrderRepository();
            orderRepo.Add(new Order { ItemName = name, Quantity = quantity }); // only logging successful orders
            return new OrderResult { Status = OrderStatus.Processed, RemainingStock = item.Stock };
        }
    }
}