using FlorMoon.Models;
using FlorMoon.Repository.Interfaces;
using FlorMoon.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlorMoon.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IUserService _userService;
        public ShoppingCartService(IRepositoryWrapper repositoryWrapper, IShoppingCartRepository shoppingCartRepository, IUserService userService)
        {
            _repositoryWrapper = repositoryWrapper;
            _shoppingCartRepository = shoppingCartRepository;
            _userService = userService;
        }
        public void ClearShoppingCart(int userID)
        {
            _repositoryWrapper.ShoppingCartRepository.ClearCart(userID);
            _repositoryWrapper.Save();
        }

        public void CreateShoppingCart(ShoppingCart shoppingCart)
        {
            _repositoryWrapper.ShoppingCartRepository.CreateShoppingCart(shoppingCart);
            _repositoryWrapper.Save();
        }

        public void AddToCart(Product product, int quantity, int userId)
        {
            var cart = _repositoryWrapper.ShoppingCartRepository.GetShoppingCartByUserID(userId);
            if (cart == null)
            {
                cart = new ShoppingCart
                {
                    userID = userId,
                    ShoppingCartItems = new List<ShoppingCartItem>()
                };
                _repositoryWrapper.ShoppingCartRepository.CreateShoppingCart(cart);
                _repositoryWrapper.Save();
            }

            var existingItem = _repositoryWrapper.ShoppingCartItemRepository
                .GetItemByProductId(product.ProductID, cart.ShoppingCartID);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                var newItem = new ShoppingCartItem
                {
                    productID = product.ProductID,
                    shoppingCartID = cart.ShoppingCartID,
                    Quantity = quantity,
                    Price = product.Price
                };
                _repositoryWrapper.ShoppingCartItemRepository.Create(newItem);
            }

            _repositoryWrapper.Save();
        }

        public ShoppingCart GetShoppingCartByUserID(int userID)
        {
            return _repositoryWrapper.ShoppingCartRepository
                .FindByCondition(c => c.userID == userID)
                .Include(c => c.ShoppingCartItems)
                .ThenInclude(i => i.Products)
                .FirstOrDefault();
        }

        public float GetTotalPrice(int userID)
        {
            return _repositoryWrapper.ShoppingCartRepository.GetTotalPrice(userID);
        }

        public void RemoveItemFromCart(int userId, int productId)
        {
            var cart = _repositoryWrapper.ShoppingCartRepository.GetShoppingCartByUserID(userId);
            if (cart == null) return;

            var item = _repositoryWrapper.ShoppingCartItemRepository.GetItemByProductId(cart.ShoppingCartID, productId);
            if (item == null) return;

            _repositoryWrapper.ShoppingCartItemRepository.RemoveItem(item);
            _repositoryWrapper.Save();
        }

        public void RemoveItemFromCart(ShoppingCartItem item)
        {
            if (item != null)
            {
                _repositoryWrapper.ShoppingCartItemRepository.RemoveItem(item);
                _repositoryWrapper.Save();
            }
        }

        public void UpdateItemQuantity(int userId, int productId, int quantity)
        {
            var cart = _repositoryWrapper.ShoppingCartRepository.GetShoppingCartByUserID(userId);
            if (cart == null) return;

            var item = _repositoryWrapper.ShoppingCartItemRepository
                .GetItemByProductId(productId, cart.ShoppingCartID);

            if (item != null)
            {
                item.Quantity = quantity;
                _repositoryWrapper.ShoppingCartItemRepository.Update(item);
                _repositoryWrapper.Save();
            }
        }

        public IEnumerable<ShoppingCartItem> GetCartItems()
        {
            var user = _userService.GetCurrentUser();
            if (user == null)
            {
                return new List<ShoppingCartItem>();
            }

            return _shoppingCartRepository.GetCartItemsByUserId(user.UserID);
        }

        public void Update(ShoppingCartItem item)
        {
            _repositoryWrapper.ShoppingCartItemRepository.Update(item);
            _repositoryWrapper.Save();
        }

        public void Save()
        {
            _repositoryWrapper.Save();
        }
    }
}
