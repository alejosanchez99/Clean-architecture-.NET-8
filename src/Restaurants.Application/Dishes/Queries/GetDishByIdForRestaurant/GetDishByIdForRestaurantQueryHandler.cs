using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Queries.GetDishByIdForRestaurant;

public class GetDishByIdForRestaurantQueryHandler(ILogger<GetDishByIdForRestaurantQueryHandler> logger,
                                                  IRestaurantsRepository restaurantsRepository,
                                                  IMapper mapper) : IRequestHandler<GetDishByIdForRestaurantQuery, DishDto>
{
    public async Task<DishDto> Handle(GetDishByIdForRestaurantQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving dish: {DishId}, for restaurant with id: {RestaurantId}", request.DishId, request.RestaurantId);

        Restaurant restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId)
                                  ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

        Dish dish = restaurant.Dishes.FirstOrDefault(dish => dish.Id == request.DishId)
                    ?? throw new NotFoundException(nameof(Dish), request.DishId.ToString());

        return mapper.Map<DishDto>(dish);
    }
}
