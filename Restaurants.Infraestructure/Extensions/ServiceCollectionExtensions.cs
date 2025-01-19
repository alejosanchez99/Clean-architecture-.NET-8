using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Domain.Repositories;
using Restaurants.Infraestructure.Persistence;
using Restaurants.Infraestructure.Repositories;
using Restaurants.Infraestructure.Seeders;

namespace Restaurants.Infraestructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("RestaurantsDb");
        services.AddDbContext<RestauntsDbContext>(options => options.UseSqlServer(connectionString));

        services.AddScoped<IRestaurantSeeder, RestaurantSeeder>();
        services.AddScoped<IRestaurantsRespository, RestaurantsRepository>();
    }
}
