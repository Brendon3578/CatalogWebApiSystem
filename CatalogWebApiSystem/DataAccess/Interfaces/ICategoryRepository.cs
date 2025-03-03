using CatalogWebApiSystem.Application.Pagination;
using CatalogWebApiSystem.Domain.Models;

namespace CatalogWebApiSystem.DataAccess.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<PagedList<Category>> GetCategoriesAsync(CategoryParameters categoryParameters);

    }
}
