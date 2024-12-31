using CatalogWebApiSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogWebApiSystem.Context;

public class CatalogWebApiSystemContext : DbContext
{
    public CatalogWebApiSystemContext(DbContextOptions<CatalogWebApiSystemContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
}
