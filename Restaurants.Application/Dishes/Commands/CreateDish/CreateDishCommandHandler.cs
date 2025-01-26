using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.CreateDish;

public class CreateDishCommandHandler(ILogger<CreateDishCommandHandler> logger,
                                      IRestaurantsRespository restaurantsRespository,
                                      IDishesRepository dishesRepository,
                                      IMapper mapper) : IRequestHandler<CreateDishCommand, int>
{
    public async Task<int> Handle(CreateDishCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating new dish: {@DishRequest}", request);

        Restaurant restaurant = await restaurantsRespository.GetByIdAsync(request.RestaurantId)
                                ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

        Dish dish = mapper.Map<Dish>(request);

        return await dishesRepository.Create(dish);
    }
}
