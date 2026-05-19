using FlorMoon.Data;
using FlorMoon.Interfaces;
using FlorMoon.Models;
using Microsoft.EntityFrameworkCore; 

namespace FlorMoon.Repository
{
    public class WishlistRepository : RepositoryBase<WishlistItem>, IWishlistRepository
    {
        public WishlistRepository(AppDbContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public IEnumerable<WishlistItem> GetWishlistWithProducts(int userId)
        {
            return _context.WishlistItems
                .Include(w => w.Product)
                .Where(w => w.UserID == userId)
                .ToList();
        }
    }
}