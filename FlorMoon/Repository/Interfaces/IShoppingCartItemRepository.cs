using FlorMoon.Models;
namespace FlorMoon.Repository.Interfaces
{
    public interface IShoppingCartItemRepository : IRepositoryBase<ShoppingCartItem>
    {
        IEnumerable<ShoppingCartItem> GetItemsInCart(int cartId);
        ShoppingCartItem GetItemByProductId(int cartId, int productId);
        void AddItem(ShoppingCartItem item);
        void RemoveItem(ShoppingCartItem item);
    }
}
