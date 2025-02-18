using AutoMapper;
using CatalogWebApiSystem.Domain.Models;

namespace CatalogWebApiSystem.Application.DTOs.Mappings
{
    public class ModelDTOMappingProfile : Profile
    {
        public ModelDTOMappingProfile()
        {
            CreateMap<Category, CategoryDTO>().ReverseMap();

            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Product, ProductDTOUpdateRequest>().ReverseMap();
            CreateMap<Product, ProductDTOUpdateResponse>().ReverseMap();
        }
    }
}
