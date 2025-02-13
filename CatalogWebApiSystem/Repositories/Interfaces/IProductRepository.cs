using CatalogWebApiSystem.Domain.Models;

namespace CatalogWebApiSystem.Repositories.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
    }
}
