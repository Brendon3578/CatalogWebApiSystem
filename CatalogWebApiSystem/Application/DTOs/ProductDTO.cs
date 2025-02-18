using CatalogWebApiSystem.Domain.Validations;
using System.ComponentModel.DataAnnotations;

namespace CatalogWebApiSystem.Application.DTOs
{
    public record ProductDTO
    {
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
        public decimal Price { get; set; }

        public int CategoryId { get; set; }


    }
}
