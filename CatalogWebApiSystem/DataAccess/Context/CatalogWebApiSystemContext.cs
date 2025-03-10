using CatalogWebApiSystem.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CatalogWebApiSystem.DataAccess.Context;

public class CatalogWebApiSystemContext : IdentityDbContext
{
    public CatalogWebApiSystemContext(DbContextOptions<CatalogWebApiSystemContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
}
