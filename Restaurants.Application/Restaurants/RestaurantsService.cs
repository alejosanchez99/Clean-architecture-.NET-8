using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants;

internal class RestaurantsService(IRestaurantsRespository restaurantsRespository, ILogger<RestaurantsService> logger) : IRestaurantsService
{
    public async Task<IEnumerable<RestaurantDto>> GetAllRestaurants()
    {
        logger.LogInformation("Getting all restaurants");

        IEnumerable<Restaurant> restaurants = await restaurantsRespository.GetAllAsync();

        return from restaurant in restaurants
               select RestaurantDto.FromEntity(restaurant);
    }

    public async Task<RestaurantDto?> GetById(int id)
    {
        logger.LogInformation($"Getting restaurant {id}");
        Restaurant? restaurant = await restaurantsRespository.GetById(id);
        
        return RestaurantDto.FromEntity(restaurant);
    }
}
