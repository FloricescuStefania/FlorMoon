using FlorMoon.Models;

namespace FlorMoon.Services.Interfaces
{
    public interface IShoppingCartService
    {
        ShoppingCart GetShoppingCartByUserID(int userID);
        void CreateShoppingCart(ShoppingCart shoppingCart);
        void ClearShoppingCart(int userID);
        float GetTotalPrice(int userID);
        void AddToCart(Product product, int quantity, int userId);
        void RemoveItemFromCart(int userId, int productId);
        void RemoveItemFromCart(ShoppingCartItem item);
        void UpdateItemQuantity(int userId, int productId, int quantity);
        void Update(ShoppingCartItem item);
        IEnumerable<ShoppingCartItem> GetCartItems();
        void Save();
    }
}
