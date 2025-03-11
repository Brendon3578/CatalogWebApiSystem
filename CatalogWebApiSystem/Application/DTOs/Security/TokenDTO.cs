namespace CatalogWebApiSystem.Application.DTOs.Security
{
    public class TokenDTO
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
