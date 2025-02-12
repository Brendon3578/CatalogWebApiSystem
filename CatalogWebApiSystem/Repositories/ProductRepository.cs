using CatalogWebApiSystem.Context;
using CatalogWebApiSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogWebApiSystem.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly CatalogWebApiSystemContext _context;

        public ProductRepository(CatalogWebApiSystemContext context)
        {
            _context = context;
        }

        public IQueryable<Product> GetProducts() =>
            _context.Products;

        public async Task<Product?> GetProductAsync(int id) =>
            await _context.Products.FindAsync(id);


        public async Task<Product> CreateAsync(Product product)
        {
            ArgumentNullException.ThrowIfNull(product, nameof(product));

            if (await CategoryExistsAsync(product.CategoryId) == false)
                throw new ArgumentException("Category not found.", nameof(product.CategoryId));

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            ArgumentNullException.ThrowIfNull(product, nameof(product));

            if (await CategoryExistsAsync(product.CategoryId) == false)
                throw new ArgumentException("Category not found.", nameof(product.CategoryId));

            if (await ProductExistsAsync(product.ProductId))
            {
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product is null)
                return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return true;
        }

        private async Task<bool> ProductExistsAsync(int id) =>
            await _context.Products.AnyAsync(p => p.ProductId == id);

        private async Task<bool> CategoryExistsAsync(int id) =>
            await _context.Products.AnyAsync(p => p.ProductId == id);

    }
}
