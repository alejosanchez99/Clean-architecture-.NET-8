using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.DeleteDishes;

public class DeleteDishesCommandHandler(ILogger<DeleteDishesCommandHandler> logger,
                                      IRestaurantsRespository restaurantsRepository,
                                      IDishesRepository dishesRepository,
                                      IRestaurantAuthorizationService restaurantAuthorizationService) : IRequestHandler<DeleteDishesCommand>
{
    public async Task Handle(DeleteDishesCommand request, CancellationToken cancellationToken)
    {
        logger.LogWarning("Removing all dishes from restaurant: {RestaurantId}", request.RestaurantId);

        Restaurant restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId)
                                  ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

        if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Update))
        {
            throw new ForbidException();
        }

        await dishesRepository.Delete(restaurant.Dishes);
    }
}
