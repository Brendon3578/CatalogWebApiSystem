namespace CatalogWebApiSystem.Domain.Models;

// Classes anêmicas (só com propriedades)
public class Category
{
    public int CategoryId { get; set; }
    public string? Name { get; set; }
    public string? ImageUrl { get; set; }
}
