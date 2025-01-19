using AutoMapper;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants;

internal class RestaurantsService(IRestaurantsRespository restaurantsRespository,
                                  ILogger<RestaurantsService> logger,
                                  IMapper mapper) : IRestaurantsService
{
    public Task<int> Create(CreateRestaurantDto createRestaurantDto)
    {
        logger.LogInformation("Creating a new restaurant");

        Restaurant restaurant = mapper.Map<Restaurant>(createRestaurantDto);

        return restaurantsRespository.Create(restaurant);

    }

    public async Task<IEnumerable<RestaurantDto>> GetAllRestaurants()
    {
        logger.LogInformation("Getting all restaurants");

        IEnumerable<Restaurant> restaurants = await restaurantsRespository.GetAllAsync();

        return mapper.Map<IEnumerable<RestaurantDto>>(restaurants);
    }

    public async Task<RestaurantDto?> GetById(int id)
    {
        logger.LogInformation($"Getting restaurant {id}");

        Restaurant? restaurant = await restaurantsRespository.GetById(id);

        return mapper.Map<RestaurantDto>(restaurant);
    }
}
