﻿namespace CatalogWebApiSystem.DataAccess.Interfaces
{
    public interface IUnitOfWork
    {
        IProductRepository ProductRepository { get; }
        ICategoryRepository CategoryRepository { get; }

        Task CommitAsync();
    }
}
