using FlorMoon.Models;
namespace FlorMoon.Repository.Interfaces
{
    public interface IShoppingCartRepository : IRepositoryBase<ShoppingCart>
    {
        ShoppingCart GetShoppingCartByUserID(int userID);
        IEnumerable<ShoppingCartItem> GetCartItemsByUserId(int userId);
        void AddToCart(int userID, int productID);
        void CreateShoppingCart(ShoppingCart shoppingCart);
        void RemoveFromCart(int userID, int productID);
        void ClearCart(int userID);
        float GetTotalPrice(int userID);
    }
}
