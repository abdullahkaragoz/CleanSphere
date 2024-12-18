using Services.Products;

namespace Services.Categories;

public record CategoryWithProductsDto(int Id, string Name, List<ProductDto> Products);
