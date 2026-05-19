using FlorMoon.Interfaces;
using FlorMoon.Repository.Interfaces;

namespace FlorMoon.Repository.Interfaces
{
    public interface IRepositoryWrapper
    {
        IProductRepository ProductRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IOrderRepository OrderRepository { get; }
        IOrderItemRepository OrderItemRepository { get; }
        IShoppingCartRepository ShoppingCartRepository { get; }
        IShoppingCartItemRepository ShoppingCartItemRepository { get; }
        IUserRepository UserRepository { get; }
        IReviewRepository ReviewRepository { get; }
        IWishlistRepository WishlistRepository { get; }
        void Save();
    }
}
