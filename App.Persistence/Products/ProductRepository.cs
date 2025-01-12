
using App.Application.Contracts.Persistence;
using App.Domain.Entities;
using App.Persistence;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Products;

public class ProductRepository(AppDbContext context) : GenericRepsitory<Product, int>(context), IProductRepository
{
    public Task<List<Product>> GetTopPriceProductsAsync(int count)
    {
        return Context.Products.OrderByDescending(x => x.Price).Take(count).ToListAsync();
    }
}