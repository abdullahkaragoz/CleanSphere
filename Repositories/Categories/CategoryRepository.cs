
using Microsoft.EntityFrameworkCore;

namespace Repositories.Categories;
public class CategoryRepository(AppDbContext context) : GenericRepsitory<Category>(context), ICategoryRepository
{
    public Task<Category?> GetCategoryByProductsAsync(int id)
    {
       return context.Categories.Include(c => c.Products).FirstOrDefaultAsync(x=>x.Id == id);
    }

    public IQueryable<Category?> GetCategoryWithProductsAsync()
    {
        return context.Categories.Include(x => x.Products).AsQueryable();
    }
}
