using FlorMoon.Models;

namespace FlorMoon.Services.Interfaces
{
    public interface IOrderService
    {
        IEnumerable<Order> GetAllOrders();
        IEnumerable<Order> GetOrdersByUserID(int userID);
        void CreateOrder(Order order);
        Order GetOrderById(int orderId);
        void UpdateOrder(Order order);
        void CompleteOrder(Order order, int userId);

    }
}
