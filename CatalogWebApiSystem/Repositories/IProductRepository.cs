using CatalogWebApiSystem.Domain.Models;

namespace CatalogWebApiSystem.Repositories
{
    public interface IProductRepository
    {
        IQueryable<Product> GetProducts();
        Task<Product?> GetProductAsync(int id);
        Task<Product> CreateAsync(Product product);
        Task<bool> UpdateAsync(Product product);
        Task<bool> DeleteAsync(int id);
        Task<bool> ProductExistsAsync(int id);
    }
}
