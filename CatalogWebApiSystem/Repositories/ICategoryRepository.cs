using CatalogWebApiSystem.Domain.Models;

namespace CatalogWebApiSystem.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<Category?> GetCategoryAsync(int id);
        Task<Category> CreateAsync(Category category);
        Task<Category> UpdateAsync(Category category);
        Task<Category> DeleteAsync(int id);
        Task<IEnumerable<Product>> GetProductsAsync(int categoryId);
        Task<bool> CategoryExistsAsync(int categoryId);
    }
}
