using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CatalogWebApiSystem.Domain.Validations
{
    public class IsUrlAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string? stringValue = value?.ToString();

            if (string.IsNullOrEmpty(stringValue))
                return ValidationResult.Success;

            if (stringValue == null || string.IsNullOrEmpty(stringValue))
                return ValidationResult.Success;

            if (!stringValue.StartsWith("http://") && !stringValue.StartsWith("https://"))
                return new ValidationResult("URL must start with 'http://' or 'https://'.");

            var urlRegex = @"https?:\/\/(?:www\.)?[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}(?:\/\S*)?";

            var isUrlValid = Regex.IsMatch(stringValue, urlRegex)
                && Uri.TryCreate(stringValue.ToString(), UriKind.Absolute, out var uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (!isUrlValid)
                return new ValidationResult("URL provided is Invalid.");

            return ValidationResult.Success;
        }
    }
}
