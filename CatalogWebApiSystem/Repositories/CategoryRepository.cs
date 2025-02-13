using CatalogWebApiSystem.Context;
using CatalogWebApiSystem.Domain.Models;
using CatalogWebApiSystem.Repositories.Interfaces;

namespace CatalogWebApiSystem.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(CatalogWebApiSystemContext context) : base(context)
        {
        }
    }
}
