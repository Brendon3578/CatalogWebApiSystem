using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogWebApiSystem.Domain.Models;

// Classes anêmicas (só com propriedades - sem comportamento)
[Table("Categories")]
public class Category
{
    public Category()
    {
        Products = new Collection<Product>();
    }

    [Key]
    public int CategoryId { get; set; }

    [Required]
    [StringLength(80)]
    public string? Name { get; set; }

    [Required]
    [Url]
    [StringLength(256)]
    public string? ImageUrl { get; set; }

    public ICollection<Product>? Products { get; set; }
}
