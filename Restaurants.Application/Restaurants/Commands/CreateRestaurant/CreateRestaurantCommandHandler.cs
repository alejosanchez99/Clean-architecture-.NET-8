using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandHandler(IRestaurantsRespository restaurantsRespository,
                                            ILogger<CreateRestaurantCommandHandler> logger,
                                            IMapper mapper) : IRequestHandler<CreateRestaurantCommand, int>
{
    public async Task<int> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating a new restaurant");

        Restaurant restaurant = mapper.Map<Restaurant>(request);

        return await restaurantsRespository.Create(restaurant);
    }
}
