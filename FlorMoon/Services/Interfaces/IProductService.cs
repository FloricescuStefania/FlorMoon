using FlorMoon.Models;

namespace FlorMoon.Services.Interfaces
{
    public interface IProductService
    {
        List<Product> GetAllProducts();
        Product GetProductById(int id);
        void CreateProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int id);
        bool Exists(int id);
        List<Product> GetProductsByCategoryId(int categoryId);
    }
}
