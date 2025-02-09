using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Common;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryHandler(IRestaurantsRepository restaurantsRepository,
                                           ILogger<GetAllRestaurantsQueryHandler> logger,
                                           IMapper mapper) : IRequestHandler<GetAllRestaurantsQuery, PagedResult<RestaurantDto>>
{
    public async Task<PagedResult<RestaurantDto>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all restaurants");

        (IEnumerable<Restaurant> restaurants, int totalCount) = await restaurantsRepository.GetAllAsync(request.SearchPhrase,
                                                                                                         request.PageSize,
                                                                                                         request.PageNumber,
                                                                                                         request.SortBy,
                                                                                                         request.SortDirection);

        IEnumerable<RestaurantDto> restaurantsDtos = mapper.Map<IEnumerable<RestaurantDto>>(restaurants);

        return new PagedResult<RestaurantDto>(restaurantsDtos, totalCount, request.PageSize, request.PageNumber);
    }
}
