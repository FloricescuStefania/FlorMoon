using FlorMoon.Data;
using FlorMoon.Models;
using FlorMoon.Repository.Interfaces;

namespace FlorMoon.Repository
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }
    }
}
