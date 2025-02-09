using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infraestructure.Authorization.Requirements;
using Xunit;

namespace Restaurants.Infraestructure.Tests.Authorization.Requirements;

public class CreatedMultipleRestaurantsRequirementHandlerTests
{
    [Fact]
    public async void HandleRequirementAsyncTest_UserHasCreatedMultipleRestaurant_ShouldSucceed()
    {
        CurrentUser currentUser = new("1", "test@test.com", [], null, null);
        Mock<IUserContext> userContextMock = new();
        userContextMock.Setup(userContext => userContext.GetCurrentUser()).Returns(currentUser);

        List<Restaurant> restaurants =
        [
            new Restaurant
            {
                OwnerId = currentUser.Id
            },
            new Restaurant
            {
                OwnerId = currentUser.Id
            },
            new Restaurant
            {
                OwnerId = "2"
            }
        ];

        Mock<IRestaurantsRepository> restaurantsRepositoryMock = new();
        restaurantsRepositoryMock.Setup(restaurantsRepository => restaurantsRepository.GetAllAsync()).ReturnsAsync(restaurants);

        CreatedMultipleRestaurantsRequirement requirement = new(2);
        CreatedMultipleRestaurantsRequirementHandler handler = new(restaurantsRepositoryMock.Object, userContextMock.Object);
        AuthorizationHandlerContext context = new([requirement], null, null);

        await handler.HandleAsync(context);

        context.HasSucceeded.Should().BeTrue();
    }

    [Fact]
    public async void HandleRequirementAsyncTest_UserHasCreatedMultipleRestaurant_ShouldFaild()
    {
        CurrentUser currentUser = new("1", "test@test.com", [], null, null);
        Mock<IUserContext> userContextMock = new();
        userContextMock.Setup(userContext => userContext.GetCurrentUser()).Returns(currentUser);

        List<Restaurant> restaurants =
        [
            new Restaurant
            {
                OwnerId = currentUser.Id
            },
            new Restaurant
            {
                OwnerId = "2"
            }
        ];

        Mock<IRestaurantsRepository> restaurantsRepositoryMock = new();
        restaurantsRepositoryMock.Setup(restaurantsRepository => restaurantsRepository.GetAllAsync()).ReturnsAsync(restaurants);

        CreatedMultipleRestaurantsRequirement requirement = new(2);
        CreatedMultipleRestaurantsRequirementHandler handler = new(restaurantsRepositoryMock.Object, userContextMock.Object);
        AuthorizationHandlerContext context = new([requirement], null, null);

        await handler.HandleAsync(context);

        context.HasSucceeded.Should().BeFalse();
        context.HasFailed.Should().BeTrue();
    }
}