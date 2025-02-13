using CatalogWebApiSystem.Context;
using CatalogWebApiSystem.Domain.Models;
using CatalogWebApiSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CatalogWebApiSystem.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(CatalogWebApiSystemContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId) =>
            await _context.Set<Product>()
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
    }
}
