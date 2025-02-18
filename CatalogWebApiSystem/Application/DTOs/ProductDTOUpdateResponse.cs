namespace CatalogWebApiSystem.Application.DTOs
{
    public record ProductDTOUpdateResponse(
        int ProductId,
        string? Name,
        string? Description,
        string? ImageUrl,
        decimal Price,
        float Stock,
        DateTime CreatedOn,
        int CategoryId
    );
}
