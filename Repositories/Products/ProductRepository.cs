
using Microsoft.EntityFrameworkCore;

namespace Repositories.Products;

public class ProductRepository(AppDbContext context) : GenericRepsitory<Product>(context), IProductRepository
{
    public Task<List<Product>> GetTopPriceProductsAsync(int count)
    {
        return Context.Products.OrderByDescending(x=> x.Price).Take(count).ToListAsync();
    }
}