using AutoMapper;
using Repositories.Products;
using Services.Products;

namespace Services.Mapping;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
    }
}
