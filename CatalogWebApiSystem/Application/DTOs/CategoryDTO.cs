using CatalogWebApiSystem.Domain.Validations;
using System.ComponentModel.DataAnnotations;

namespace CatalogWebApiSystem.Application.DTOs
{
    public class CategoryDTO
    {
        public int CategoryId { get; set; }

        [Required]
        [StringLength(80)]
        [MaxLength(80)]
        public string? Name { get; set; }

        [Required]
        [IsUrl]
        [StringLength(256)]
        [MaxLength(256)]
        public string? ImageUrl { get; set; }
    }
}
