using CatalogWebApiSystem.DataAccess.Context;
using CatalogWebApiSystem.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CatalogWebApiSystem.DataAccess.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly CatalogWebApiSystemContext _context;

        public Repository(CatalogWebApiSystemContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync() =>
            await _context.Set<T>().AsNoTracking().ToListAsync();

        public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate) =>
            await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate);

        public async Task<T> CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            await Task.Run(() => _context.Set<T>().Update(entity));
            return entity;
        }

        public async Task<T> DeleteAsync(T entity)
        {
            await Task.Run(() => _context.Set<T>().Remove(entity));
            return entity;
        }

        public async Task<T?> GetByIdAsync(object id) =>
            await _context.Set<T>().FindAsync(id);
    }
}
