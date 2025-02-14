using CatalogWebApiSystem.DataAccess.Context;
using CatalogWebApiSystem.DataAccess.Interfaces;

namespace CatalogWebApiSystem.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private IProductRepository? _productRepository;

        private ICategoryRepository? _categoryRepository;

        public CatalogWebApiSystemContext _context;

        private bool _disposed = false;

        public UnitOfWork(CatalogWebApiSystemContext context)
        {
            _context = context;
        }

        public IProductRepository ProductRepository =>
            _productRepository ??= new ProductRepository(_context);

        public ICategoryRepository CategoryRepository =>
            _categoryRepository ??= new CategoryRepository(_context);

        public async Task CommitAsync() =>
            await _context.SaveChangesAsync();


        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
                _context.Dispose();

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
