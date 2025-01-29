﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infraestructure.Authorization;
using Restaurants.Infraestructure.Persistence;
using Restaurants.Infraestructure.Repositories;
using Restaurants.Infraestructure.Seeders;

namespace Restaurants.Infraestructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("RestaurantsDb");
        services.AddDbContext<RestaurantsDbContext>(options => options.UseSqlServer(connectionString)
                                                                      .EnableSensitiveDataLogging());

        services.AddIdentityApiEndpoints<User>()
                .AddRoles<IdentityRole>()
                .AddClaimsPrincipalFactory<RestaurantUserClaimsPrincipalFactory>()
                .AddEntityFrameworkStores<RestaurantsDbContext>();

        services.AddScoped<IRestaurantSeeder, RestaurantSeeder>();
        services.AddScoped<IRestaurantsRespository, RestaurantsRepository>();
        services.AddScoped<IDishesRepository, DishesRepository>();

        services.AddAuthorizationBuilder()
                .AddPolicy(PolicyNames.HasNationality, builder => builder.RequireClaim(AppClaimTypes.Nationality, "German", "Polish"));
    }
}
