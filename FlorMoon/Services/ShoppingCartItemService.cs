using FlorMoon.Models;
using FlorMoon.Repository.Interfaces;
using FlorMoon.Services.Interfaces;

namespace FlorMoon.Services
{
    public class ShoppingCartItemService : IShoppingCartItemService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        public ShoppingCartItemService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
        public void AddItem(ShoppingCartItem item)
        {
            _repositoryWrapper.ShoppingCartItemRepository.AddItem(item);
            _repositoryWrapper.Save();
        }

        public IEnumerable<ShoppingCartItem> GetAllItems(int shoppingCartID)
        {
            return _repositoryWrapper.ShoppingCartItemRepository.GetItemsInCart(shoppingCartID);
        }

        public void RemoveItem(int itemID)
        {
            var item = _repositoryWrapper.ShoppingCartItemRepository.FindByCondition(i => i.ShoppingCartItemID == itemID).FirstOrDefault();
            if (item != null)
            {
                _repositoryWrapper.ShoppingCartItemRepository.Delete(item);
                _repositoryWrapper.Save();
            }
        }

        public void RemoveItem(ShoppingCartItem item)
        {
            if (item != null)
            {
                _repositoryWrapper.ShoppingCartItemRepository.RemoveItem(item);
                _repositoryWrapper.Save();
            }
        }

        public void UpdateQuantity(int itemID, int quantity)
        {
            var item = _repositoryWrapper.ShoppingCartItemRepository.FindByCondition(i => i.ShoppingCartItemID == itemID).FirstOrDefault();
            if (item != null)
            {
                item.Quantity = quantity;
                item.Price = item.Products.Price * quantity;
                _repositoryWrapper.ShoppingCartItemRepository.Update(item);
                _repositoryWrapper.Save();
            }
        }
    }
}
