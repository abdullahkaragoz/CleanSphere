using App.Application.Contracts.Persistence;
using App.Domain.Options;
using App.Persistence.Categories;
using App.Persistence.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repositories.Interceptors;

namespace App.Persistence.Extensions;

public static class RepositoryExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(opt =>
        {
            var connectionStrings = configuration
                .GetSection(ConnectionStringOption.Key)
                .Get<ConnectionStringOption>();

            opt.UseSqlServer(connectionStrings!.SqlServer, sqlServerOptionsAction =>
            {
                sqlServerOptionsAction.MigrationsAssembly(typeof(PersistenceAssembly).Assembly.FullName);
            });

            opt.AddInterceptors(new AuditDbContextInterceptor());
        });
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepsitory<,>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
