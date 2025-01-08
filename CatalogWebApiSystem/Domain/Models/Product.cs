using CatalogWebApiSystem.Domain.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CatalogWebApiSystem.Domain.Models;

[Table("Products")]
public class Product
{
    [Key]
    public int ProductId { get; set; }

    [Required]
    [StringLength(80)]
    [MaxLength(80)]
    public string? Name { get; set; }

    [Required]
    [StringLength(256)]
    [MaxLength(256)]
    public string? Description { get; set; }

    [Required]
    [IsUrl]
    [StringLength(256)]
    [MaxLength(256)]
    public string? ImageUrl { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    public float Stock { get; set; }

    public DateTime CreatedOn { get; set; }

    public int CategoryId { get; set; }

    [JsonIgnore]
    public Category? Category { get; set; }
}
