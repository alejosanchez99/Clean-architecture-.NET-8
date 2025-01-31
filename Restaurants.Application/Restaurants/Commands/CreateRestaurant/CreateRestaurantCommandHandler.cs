using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandHandler(IRestaurantsRespository restaurantsRespository,
                                            ILogger<CreateRestaurantCommandHandler> logger,
                                            IMapper mapper,
                                            IUserContext userContext) : IRequestHandler<CreateRestaurantCommand, int>
{
    public async Task<int> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        CurrentUser? currentUser = userContext.GetCurrentUser();

        logger.LogInformation("{UserEmail} [{UserId}] Creating a new restaurant {@Restaurant}", currentUser.Email, currentUser.Id, request);

        Restaurant restaurant = mapper.Map<Restaurant>(request);
        restaurant.OwnerId = currentUser.Id;

        return await restaurantsRespository.Create(restaurant);
    }
}
