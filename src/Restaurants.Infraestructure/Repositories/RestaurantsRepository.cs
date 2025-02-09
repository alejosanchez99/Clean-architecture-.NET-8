using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infraestructure.Persistence;
using System.Linq.Expressions;

namespace Restaurants.Infraestructure.Repositories;

internal class RestaurantsRepository(RestaurantsDbContext dbContext) : IRestaurantsRepository
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

    public async Task<(IEnumerable<Restaurant>, int)> GetAllAsync(string? searchPhrase, int pageSize, int pageNumber, string? sortBy, SortDirection sortDirection)
    {
        IQueryable<Restaurant> baseQuery = dbContext.Restaurants
                                           .Where(restaurant => searchPhrase == null || restaurant.Name.Contains(searchPhrase) || restaurant.Description.Contains(searchPhrase));

        int totalCount = await baseQuery.CountAsync();

        if (sortBy != null)
        {
            Dictionary<string, Expression<Func<Restaurant, object>>> columnsSelector = new()
            {
                    { nameof(Restaurant.Name), restaurant => restaurant.Name },
                    { nameof(Restaurant.Description), restaurant => restaurant.Description },
                    { nameof(Restaurant.Category), restaurant => restaurant.Category }
            };

            Expression<Func<Restaurant, object>> selectedColumn = columnsSelector[sortBy];

            baseQuery = sortDirection == SortDirection.Ascending ? baseQuery.OrderBy(selectedColumn) : baseQuery.OrderByDescending(selectedColumn);
        }

        List<Restaurant> restaurants = await baseQuery
                                      .Skip(pageSize * (pageNumber - 1))
                                      .Take(pageSize)
                                      .ToListAsync();

        return (restaurants, totalCount);
    }

    public async Task<Restaurant?> GetByIdAsync(int id)
    {
        return await dbContext.Restaurants
            .Include(restaurant => restaurant.Dishes)
            .FirstOrDefaultAsync(restaurant => restaurant.Id == id);
    }

    public Task SaveChanges() => dbContext.SaveChangesAsync();
}
