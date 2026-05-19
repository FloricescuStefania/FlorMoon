using FlorMoon.Models;
using FlorMoon.Repository.Interfaces;
using FlorMoon.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlorMoon.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public OrderService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _repositoryWrapper.OrderRepository.FindAll();
        }

        public IEnumerable<Order> GetOrdersByUserID(int userID)
        {
            return _repositoryWrapper.OrderRepository.GetOrdersByUserID(userID);
        }

        public void CreateOrder(Order order)
        {
            _repositoryWrapper.OrderRepository.Create(order);
            _repositoryWrapper.Save();
        }

        public Order GetOrderById(int orderId)
        {
            return _repositoryWrapper.OrderRepository.GetOrderByOrderID(orderId);
        }

        public void UpdateOrder(Order order)
        {
            _repositoryWrapper.OrderRepository.Update(order);
            _repositoryWrapper.Save();
        }

        public void CompleteOrder(Order order, int userId)
        {
            var userCart = _repositoryWrapper.ShoppingCartRepository
                .FindByCondition(c => c.userID == userId)
                .AsEnumerable()
                .FirstOrDefault();

            if (userCart == null)
            {
                throw new InvalidOperationException("No active shopping cart was found for this user.");
            }

            int cartId = userCart.ShoppingCartID;

            var cartItems = _repositoryWrapper.ShoppingCartItemRepository
                .FindByCondition(item => item.shoppingCartID == cartId)
                .ToList();

            if (!cartItems.Any())
            {
                throw new InvalidOperationException("The shopping cart is empty.");
            }

            var newOrder = new Order
            {
                UserID = userId,
                Status = "Pending",
                OrderPrice = order.OrderPrice,
                OrderDate = DateTime.Now
            };

            if (order.ShippingDetails != null)
            {
                newOrder.ShippingDetails = new ShippingDetail
                {
                    Address = order.ShippingDetails.Address
                };
            }

            newOrder.OrderItems = order.OrderItems.Select(item => new OrderItem
            {
                ProductID = item.ProductID,
                Quantity = item.Quantity,
                Price = item.Price
            }).ToList();

            _repositoryWrapper.OrderRepository.Create(newOrder);
            _repositoryWrapper.Save();

            order.OrderID = newOrder.OrderID;

            foreach (var item in cartItems)
            {
                _repositoryWrapper.ShoppingCartItemRepository.Delete(item);
            }

            _repositoryWrapper.Save();
        }
    }
}