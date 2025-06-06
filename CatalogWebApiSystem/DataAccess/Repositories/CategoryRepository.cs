﻿using CatalogWebApiSystem.Application.Pagination;
using CatalogWebApiSystem.Application.Pagination.Category;
using CatalogWebApiSystem.DataAccess.Context;
using CatalogWebApiSystem.DataAccess.Interfaces;
using CatalogWebApiSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogWebApiSystem.DataAccess.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(CatalogWebApiSystemContext context) : base(context)
        {
        }

        public async Task<PagedList<Category>> GetCategoriesAsync(CategoryParameters categoryParameters)
        {
            var categories = _context.Set<Category>()
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .AsQueryable();

            return await PagedList<Category>.ToPagedListAsync(categories, categoryParameters.PageNumber, categoryParameters.PageSize);
        }

        public async Task<PagedList<Category>> GetCategoriesByNameAsync(CategoryFilterNameParameter categoryParameters)
        {
            var categories = _context.Set<Category>()
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .AsQueryable();


            if (!string.IsNullOrEmpty(categoryParameters.Name))
            {
                categories = categories.Where(c => c.Name != null && c.Name.Contains(categoryParameters.Name));
            }

            return await PagedList<Category>.ToPagedListAsync(categories, categoryParameters.PageNumber, categoryParameters.PageSize);
        }
    }
}
