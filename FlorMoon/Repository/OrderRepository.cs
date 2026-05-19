using FlorMoon.Data;
using FlorMoon.Models;
using FlorMoon.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlorMoon.Repository
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {
        }

        public Order GetOrderByOrderID(int orderID)
        {
            return _context.Orders
                   .Include(o => o.OrderItems)
                       .ThenInclude(oi => oi.Products)
                   .Include(o => o.ShippingDetails)
                   .Include(o => o.Users)
                   .FirstOrDefault(o => o.OrderID == orderID);
        }

        public IEnumerable<Order> GetOrdersByUserID(int userID)
        {
            return FindByCondition(o => o.UserID == userID).ToList();
        }
    }
}
