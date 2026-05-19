using FlorMoon.Models;
using FlorMoon.Repository.Interfaces;
using FlorMoon.Services.Interfaces;

namespace FlorMoon.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public OrderItemService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public void CreateOrderItem(OrderItem orderItem)
        {
            _repositoryWrapper.OrderItemRepository.CreateOrderItem(orderItem);
            _repositoryWrapper.Save();
        }

        public void DeleteOrderItem(int orderItemID)
        {
            var item = _repositoryWrapper.OrderItemRepository
                .FindByCondition(o => o.OrderItemsID == orderItemID)
                .FirstOrDefault();
            if (item != null)
            {
                _repositoryWrapper.OrderItemRepository.Delete(item);
                _repositoryWrapper.Save();
            }
        }

        public IEnumerable<OrderItem> GetOrderItemsByOrderID(int orderID)
        {
            return _repositoryWrapper.OrderItemRepository.GetOrderItemsByOrderID(orderID);
        }
    }
}
