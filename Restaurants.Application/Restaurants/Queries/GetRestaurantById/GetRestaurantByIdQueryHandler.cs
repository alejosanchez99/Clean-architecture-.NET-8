using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetRestaurantById;

public class GetRestaurantByIdQueryHandler(IRestaurantsRespository restaurantsRespository,
                                           ILogger<GetRestaurantByIdQueryHandler> logger,
                                           IMapper mapper) : IRequestHandler<GetRestaurantByIdQuery, RestaurantDto>
{
    public async Task<RestaurantDto> Handle(GetRestaurantByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting restaurant {RestaurantId}", request.Id);

        Restaurant? restaurant = await restaurantsRespository.GetByIdAsync(request.Id);

        return mapper.Map<RestaurantDto>(restaurant);
    }
}
