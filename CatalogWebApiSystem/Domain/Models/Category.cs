using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace CatalogWebApiSystem.Domain.Models;

// Classes anêmicas (só com propriedades - sem comportamento)
[Table("Categories")]
public class Category : IValidatableObject
{
    public Category()
    {
        Products = new Collection<Product>();
    }

    [Key]
    public int CategoryId { get; set; }

    [Required]
    [StringLength(80)]
    [MaxLength(80)]
    public string? Name { get; set; }

    [Required]
    [StringLength(256)]
    [MaxLength(256)]
    public string? ImageUrl { get; set; }

    [JsonIgnore]
    public ICollection<Product>? Products { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!string.IsNullOrEmpty(ImageUrl))
        {
            string url = ImageUrl;

            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                yield return new ValidationResult("URL must start with 'http://' or 'https://'.",
                    [nameof(ImageUrl)]
                );

            var urlRegex = @"https?:\/\/(?:www\.)?[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}(?:\/\S*)?";

            var isUrlValid = Regex.IsMatch(url, urlRegex)
                && Uri.TryCreate(url.ToString(), UriKind.Absolute, out var uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (!isUrlValid)
                yield return new ValidationResult("URL provided is Invalid.",
                    [nameof(ImageUrl)]
                );
        }
    }
}
