using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandHandler(IRestaurantsRespository restaurantsRespository,
                                              ILogger<CreateRestaurantCommandHandler> logger,
                                              IMapper mapper) : IRequestHandler<UpdateRestaurantCommand, bool>
{
    public async Task<bool> Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Deleting restaurant with id: {request.Id}");
        Restaurant? restaurant = await restaurantsRespository.GetByIdAsync(request.Id);
        if (restaurant is null)
        {
            return false;
        }

        mapper.Map(request, restaurant);

        await restaurantsRespository.SaveChanges();

        return true;
    }
}
