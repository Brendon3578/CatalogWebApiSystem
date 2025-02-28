using CatalogWebApiSystem.Application.Pagination;
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

        public async Task<PagedList<Product>> GetProductsByCategoryAsync(int categoryId, ProductParameters productParams)
        {
            var products = _context.Set<Product>()
                .AsNoTracking()
                .Where(p => p.CategoryId == categoryId)
                .OrderBy(p => p.Name)
                .AsQueryable();

            var ordenedProducts = await PagedList<Product>.ToPagedListAsync(products, productParams.PageNumber, productParams.PageSize);
            return ordenedProducts;
        }

        //public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId, ProductParameters productParams) =>
        //    await _context.Set<Product>()
        //        .AsNoTracking()
        //        .Where(p => p.CategoryId == categoryId)
        //        .OrderBy(p => p.Name)
        //        .Skip((productParams.PageNumber - 1) * productParams.PageSize)
        //        .Take(productParams.PageSize)
        //        .ToListAsync();


    }
}
