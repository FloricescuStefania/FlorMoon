using FlorMoon.Data;
using FlorMoon.Models;
using FlorMoon.Repository.Interfaces;

namespace FlorMoon.Repository
{
    public class ShoppingCartItemRepository : RepositoryBase<ShoppingCartItem>, IShoppingCartItemRepository
    {
        public ShoppingCartItemRepository(AppDbContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public IEnumerable<ShoppingCartItem> GetItemsInCart(int cartId)
        {
            return FindByCondition(item => item.shoppingCartID == cartId).ToList();
        }

        public ShoppingCartItem GetItemByProductId(int productId, int cartId)
        {
            return FindByCondition(sci => sci.productID == productId && sci.shoppingCartID == cartId).FirstOrDefault();
        }

        public void AddItem(ShoppingCartItem item)
        {
            Create(item);
        }

        public void RemoveItem(ShoppingCartItem item)
        {
            Delete(item);
        }
    }
}
