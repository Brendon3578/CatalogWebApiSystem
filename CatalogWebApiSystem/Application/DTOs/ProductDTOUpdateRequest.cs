using System.ComponentModel.DataAnnotations;

namespace CatalogWebApiSystem.Application.DTOs
{
    public class ProductDTOUpdateRequest : IValidatableObject
    {
        [Range(1, 9999, ErrorMessage = "Stock must be between 1 to 9999")]
        public float Stock { get; set; }
        public DateTime CreatedOn { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CreatedOn.Date < DateTime.UtcNow.Date)
                yield return new ValidationResult("CreatedOn must be greater than the current date", [nameof(CreatedOn)]);
        }
    }
}
