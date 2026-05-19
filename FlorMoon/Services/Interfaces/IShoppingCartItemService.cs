using FlorMoon.Models;

namespace FlorMoon.Services.Interfaces
{
    public interface IShoppingCartItemService
    {
        IEnumerable<ShoppingCartItem> GetAllItems(int shoppingCartID);
        void AddItem(ShoppingCartItem item);
        void RemoveItem(int itemID);
        void UpdateQuantity(int itemID, int quantity);
        void RemoveItem(ShoppingCartItem item);
    }
}
