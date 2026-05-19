using FlorMoon.Models;

namespace FlorMoon.Repository.Interfaces
{
    public interface IOrderItemRepository : IRepositoryBase<OrderItem>
    {
        IEnumerable<OrderItem> GetOrderItemsByOrderID(int orderID);
        void CreateOrderItem(OrderItem orderItem);
        void Delete(OrderItem orderItem);
    }
}
