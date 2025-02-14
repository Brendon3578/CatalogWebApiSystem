using CatalogWebApiSystem.DataAccess.Context;
using CatalogWebApiSystem.DataAccess.Interfaces;
using CatalogWebApiSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogWebApiSystem.DataAccess.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(CatalogWebApiSystemContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId) =>
            await _context.Set<Product>()
                .AsNoTracking()
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
    }
}
