using CatalogWebApiSystem.Domain.Models.Interfaces;
using System.Linq.Expressions;

namespace CatalogWebApiSystem.Repositories.Interfaces
{
    public interface IRepository<T> where T : IEntityBase
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetAsync(Expression<Func<T, bool>> predicate);
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> DeleteAsync(T entity);
        Task<bool> ExistsAsync(int id);
    }
}
