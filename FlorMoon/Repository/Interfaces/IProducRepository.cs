using FlorMoon.Models;

namespace FlorMoon.Repository.Interfaces
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        public List<Product> GetAllWithCategoryAndProvider();
    }
}
