using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infraestructure.Persistence;

namespace Restaurants.Infraestructure.Repositories;

internal class RestaurantsRepository(RestaurantsDbContext dbContext) : IRestaurantsRespository
{
    public async Task<int> Create(Restaurant entity)
    {
        dbContext.Restaurants.Add(entity);
        await dbContext.SaveChangesAsync();

        return entity.Id;
    }

    public async Task Delete(Restaurant entity)
    {
        dbContext.Remove(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Restaurant>> GetAllAsync()
    {
        return await dbContext.Restaurants.ToListAsync();
    }

    public async Task<(IEnumerable<Restaurant>, int)> GetAllAsync(string? searchPhrase, int pageSize, int pageNumber)
    {
        IQueryable<Restaurant> baseQuery = dbContext.Restaurants
                                           .Where(restaurant => searchPhrase == null || restaurant.Name.Contains(searchPhrase) || restaurant.Description.Contains(searchPhrase));

        int totalCount = await baseQuery.CountAsync();

        List<Restaurant> restaurants = await baseQuery
                                      .Skip(pageSize * (pageNumber - 1))
                                      .Take(pageSize)
                                      .ToListAsync();

        return (restaurants,  totalCount);
    }

    public async Task<Restaurant?> GetByIdAsync(int id)
    {
        return await dbContext.Restaurants
            .Include(restaurant => restaurant.Dishes)
            .FirstOrDefaultAsync(restaurant => restaurant.Id == id);
    }

    public Task SaveChanges() => dbContext.SaveChangesAsync();
}
