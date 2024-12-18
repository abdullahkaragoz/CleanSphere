namespace Repositories.Categories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<Category?> GetCategoryByProductsAsync(int id);
        IQueryable<Category?> GetCategoryWithProductsAsync();
    }
}
