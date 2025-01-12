namespace App.Application.Features.Products;

public record ProductDto(int id, string Name, decimal Price, int Stock, int CategoryId);
