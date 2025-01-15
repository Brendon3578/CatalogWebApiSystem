using System.Text.Json;

namespace CatalogWebApiSystem.Domain.Models
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public string? Trace { get; set; }

        public override string ToString() => JsonSerializer.Serialize(this);

    }
}
