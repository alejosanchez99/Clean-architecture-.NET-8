using Microsoft.AspNetCore.Authorization;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Infraestructure.Authorization.Requirements;

internal class CreatedMultipleRestaurantsRequirementHandler(IRestaurantsRepository restaurantsRepository,
                                                          IUserContext userContext) : AuthorizationHandler<CreatedMultipleRestaurantsRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CreatedMultipleRestaurantsRequirement requirement)
    {
        CurrentUser currentUser = userContext.GetCurrentUser()!;

        IEnumerable<Restaurant> restaurants = await restaurantsRepository.GetAllAsync();

        int userRestaurantsCreated = restaurants.Count(restaurant => restaurant.OwnerId == currentUser.Id);

        if (userRestaurantsCreated >= requirement.MinimumRestaurantsCreated)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }
}
