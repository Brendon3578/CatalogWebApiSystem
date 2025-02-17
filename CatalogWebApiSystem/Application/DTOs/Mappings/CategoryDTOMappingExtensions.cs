using CatalogWebApiSystem.Domain.Models;

namespace CatalogWebApiSystem.Application.DTOs.Mappings
{
    public static class CategoryDTOMappingExtensions
    {
        public static CategoryDTO? ToCategoryDTO(this Category? category) =>
            category != null ? new CategoryDTO()
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                ImageUrl = category.ImageUrl
            } : null;

        public static Category? ToCategory(this CategoryDTO? categoryDTO) =>
            categoryDTO != null ? new Category()
            {
                CategoryId = categoryDTO.CategoryId,
                Name = categoryDTO.Name,
                ImageUrl = categoryDTO.ImageUrl
            } : null;

        public static IEnumerable<CategoryDTO?>? ToCategoryDTOList(this IEnumerable<Category>? categories)
        {
            if (categories is null || !categories.Any())
                return [];

            return categories.Select(c => c.ToCategoryDTO());
        }
    }
}
