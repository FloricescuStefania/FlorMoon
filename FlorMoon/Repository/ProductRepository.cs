using FlorMoon.Data;
using FlorMoon.Models;
using FlorMoon.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlorMoon.Repository
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public List<Product> GetAllWithCategoryAndProvider()
        {
            return _context.Products
                .Include(p => p.Categories)
                .ToList();
        }
    }
}
