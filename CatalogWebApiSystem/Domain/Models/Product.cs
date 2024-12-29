namespace CatalogWebApiSystem.Domain.Models;

public class Product
{
    public int ProductId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal Price { get; set; }
    public float Stock { get; set; }
    public DateTime CreatedOn { get; set; }

    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}
