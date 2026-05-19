using FlorMoon.Models;
using FlorMoon.Repository.Interfaces;
using System.Collections.Generic;

namespace FlorMoon.Interfaces
{
    public interface IWishlistRepository : IRepositoryBase<WishlistItem>
    {
        IEnumerable<WishlistItem> GetWishlistWithProducts(int userId);
    }
}