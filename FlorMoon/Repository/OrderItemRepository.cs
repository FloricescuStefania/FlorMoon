using FlorMoon.Data;
using FlorMoon.Models;
using FlorMoon.Repository.Interfaces;

namespace FlorMoon.Repository
{
    public class OrderItemRepository : RepositoryBase<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(AppDbContext context) : base(context)
        {
        }

        public IEnumerable<OrderItem> GetOrderItemsByOrderID(int orderID)
        {
            return _context.OrderItems.Where(oi => oi.OrderID == orderID).ToList();
        }

        public void CreateOrderItem(OrderItem orderItem)
        {
            Create(orderItem);
        }
        public void DeleteOrderItem(OrderItem orderItem)
        {
            Delete(orderItem);
        }
    }
}
