using CatalogWebApiSystem.Context;
using CatalogWebApiSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogWebApiSystem.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly CatalogWebApiSystemContext _context;
        public CategoryRepository(CatalogWebApiSystemContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _context.Categories.AsNoTracking().ToListAsync();
        }

        public async Task<Category?> GetCategoryAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<Category> CreateAsync(Category category)
        {
            if (category is null)
                throw new ArgumentNullException(nameof(category));

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return category;
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            if (category is null)
                throw new ArgumentNullException(nameof(category));

            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return category;
        }

        public async Task<Category> DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category is null)
                throw new ArgumentNullException(nameof(category));

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return category;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(int categoryId)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<bool> CategoryExistsAsync(int categoryId) =>
            await _context.Categories
                .AsNoTracking()
                .AnyAsync(e => e.CategoryId == categoryId);
    }
}
