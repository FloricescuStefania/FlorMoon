using FlorMoon.Interfaces;
using FlorMoon.Models;
using FlorMoon.Repository.Interfaces;

namespace FlorMoon.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IRepositoryWrapper _repoWrapper;

        public WishlistService(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        public IEnumerable<WishlistItem> GetUserWishlist(int userId)
        {
            return _repoWrapper.WishlistRepository.GetWishlistWithProducts(userId);
        }

        public void AddToWishlist(int userId, int productId)
        {
            if (!IsInWishlist(userId, productId))
            {
                var item = new WishlistItem
                {
                    UserID = userId,
                    ProductID = productId,
                    AddedAt = DateTime.Now
                };

                _repoWrapper.WishlistRepository.Create(item);
                _repoWrapper.Save();
            }
        }
        public void RemoveFromWishlist(int userId, int productId)
        {
            var item = _repoWrapper.WishlistRepository
                .FindByCondition(w => w.UserID == userId && w.ProductID == productId)
                .FirstOrDefault();

            if (item != null)
            {
                _repoWrapper.WishlistRepository.Delete(item);
                _repoWrapper.Save();
            }
        }

        public bool IsInWishlist(int userId, int productId)
        {
            return _repoWrapper.WishlistRepository
                .FindByCondition(w => w.UserID == userId && w.ProductID == productId)
                .Any();
        }
    }
}