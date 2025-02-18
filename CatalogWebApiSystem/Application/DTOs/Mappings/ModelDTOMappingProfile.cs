using AutoMapper;
using CatalogWebApiSystem.Domain.Models;

namespace CatalogWebApiSystem.Application.DTOs.Mappings
{
    public class ModelDTOMappingProfile : Profile
    {
        public ModelDTOMappingProfile()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
        }
    }
}
