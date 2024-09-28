using Dishes.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class PersistenceServiceRegistrationExtensions
{
    private const string ConnectionString = "DishesDbConnection";

    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add DbContext
        services.AddDbContext<DishesDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString(ConnectionString)));

        // Add Repositories
       
        return services;
    }
}