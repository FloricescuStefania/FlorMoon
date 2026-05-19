using FlorMoon.Models;

namespace FlorMoon.Repository.Interfaces
{
    public interface IOrderRepository : IRepositoryBase<Order>
    {
        IEnumerable<Order> GetOrdersByUserID(int userID);
        Order GetOrderByOrderID(int orderID);
    }
}
