using CatalogWebApiSystem.Domain.Models;

namespace CatalogWebApiSystem.DataAccess.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
    }
}
