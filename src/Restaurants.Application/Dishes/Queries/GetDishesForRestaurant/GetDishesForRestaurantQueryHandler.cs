﻿using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Queries.GetDishesForRestaurant;

public class GetDishesForRestaurantQueryHandler(ILogger<GetDishesForRestaurantQueryHandler> logger,
                                                IRestaurantsRepository restaurantsRepository,
                                                IMapper mapper) : IRequestHandler<GetDishesForRestaurantQuery, IEnumerable<DishDto>>
{
    public async Task<IEnumerable<DishDto>> Handle(GetDishesForRestaurantQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving dishes for restaurant with id: {RestaurantId}", request.RestaurantId);

        Restaurant restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId)
                                  ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

        return mapper.Map<IEnumerable<DishDto>>(restaurant.Dishes);
    }
}
