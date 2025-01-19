using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;

namespace Restaurants.Infraestructure.Persistence;

internal class RestauntsDbContext(DbContextOptions<RestauntsDbContext> options) : DbContext(options)
{
    internal DbSet<Restaurant> Restaurants { get; set; }
    internal DbSet<Dish> Dishes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Restaurant>()
                    .OwnsOne(restaurant => restaurant.Address);

        modelBuilder.Entity<Restaurant>()
                    .HasMany(restaurant => restaurant.Dishes)
                    .WithOne()
                    .HasForeignKey(dish => dish.RestaurantId);
    }
}
