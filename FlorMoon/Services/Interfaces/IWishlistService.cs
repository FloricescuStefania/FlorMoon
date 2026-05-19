using FlorMoon.Models;

namespace FlorMoon.Services
{
    public interface IWishlistService
    {
        IEnumerable<WishlistItem> GetUserWishlist(int userId);
        void AddToWishlist(int userId, int productId);
        void RemoveFromWishlist(int userId, int productId);
        bool IsInWishlist(int userId, int productId);
    }
}