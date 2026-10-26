using AutoMapper;
using ProductApi.Domain.Entities;
using ProductApi.Application.Dtos;

namespace ProductApi.Application.Mappings;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(
                dest => dest.CategoryName,
                opt => opt.MapFrom(src =>
                    src.Category != null
                    ? src.Category.Name
                    : string.Empty)
                );
    }
}

