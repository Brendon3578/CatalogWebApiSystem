using CatalogWebApiSystem.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CatalogWebApiSystem.DataAccess.Context;

public class CatalogWebApiSystemContext : IdentityDbContext<ApplicationUser>
{
    public CatalogWebApiSystemContext(DbContextOptions<CatalogWebApiSystemContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
