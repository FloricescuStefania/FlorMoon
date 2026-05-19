using FlorMoon.Models;

namespace FlorMoon.Services.Interfaces
{
    public interface IOrderItemService
    {
        IEnumerable<OrderItem> GetOrderItemsByOrderID(int orderID);
        void CreateOrderItem(OrderItem orderItem);
        void DeleteOrderItem(int orderItemID);
    }
}
