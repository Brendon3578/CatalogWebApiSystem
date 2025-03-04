using CatalogWebApiSystem.Application.Pagination;
using CatalogWebApiSystem.Application.Pagination.Product;
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
                .ThenBy(p => p.Price)
                .AsQueryable();

            return await PagedList<Product>.ToPagedListAsync(products, productParams.PageNumber, productParams.PageSize);
        }


        public async Task<PagedList<Product>> GetProductsAsync(ProductParameters productParams)
        {
            var products = _context.Set<Product>()
                .AsNoTracking()
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Price)
                .AsQueryable();

            return await PagedList<Product>.ToPagedListAsync(products, productParams.PageNumber, productParams.PageSize);
        }

        public async Task<PagedList<Product>> GetProductsByPriceRangeAsync(ProductFilterPriceParemeter productParams)
        {
            var products = _context.Set<Product>()
                .AsNoTracking()
                .OrderBy(p => p.Name)
                .AsQueryable();

            if (productParams.MinPrice.HasValue)
                products = products.Where(p => p.Price >= productParams.MinPrice);
            if (productParams.MaxPrice.HasValue)
                products = products.Where(p => p.Price <= productParams.MaxPrice);
            if (productParams.EqualsPrice.HasValue)
                products = products.Where(p => p.Price == productParams.EqualsPrice);

            return await PagedList<Product>.ToPagedListAsync(products, productParams.PageNumber, productParams.PageSize);
        }
    }
}
