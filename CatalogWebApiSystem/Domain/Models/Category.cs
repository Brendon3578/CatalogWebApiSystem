using System.Collections.ObjectModel;

namespace CatalogWebApiSystem.Domain.Models;

// Classes anêmicas (só com propriedades - sem comportamento)
public class Category
{
    public Category()
    {
        Products = new Collection<Product>();
    }

    public int CategoryId { get; set; }
    public string? Name { get; set; }
    public string? ImageUrl { get; set; }

    public ICollection<Product>? Products { get; set; }
}
