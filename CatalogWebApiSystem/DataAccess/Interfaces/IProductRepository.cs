﻿using CatalogWebApiSystem.Application.Pagination;
using CatalogWebApiSystem.Domain.Models;

namespace CatalogWebApiSystem.DataAccess.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId, ProductParameters productParams);
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
    }
}
