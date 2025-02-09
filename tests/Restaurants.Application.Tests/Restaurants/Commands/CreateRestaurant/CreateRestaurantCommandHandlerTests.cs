using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Application.Tests.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandHandlerTests
{
    [Fact]
    public async Task Handle_ForValidCommand_ReturnsCreatedRestaurantIdAsync()
    {
        Mock<ILogger<CreateRestaurantCommandHandler>> loggerMock = new();
        Mock<IMapper> mapperMock = new();
        Mock<IRestaurantsRepository> restaurantRepositoryMock = new();
        Mock<IUserContext> userContextMock = new();

        CreateRestaurantCommand command = new();
        Restaurant restaurant = new();

        mapperMock.Setup(mapper => mapper.Map<Restaurant>(command)).Returns(restaurant);

        restaurantRepositoryMock.Setup(repository => repository.Create(It.IsAny<Restaurant>()))
                                .ReturnsAsync(1);

        CurrentUser currentUser = new("owner-id", "test@test.com", [], null, null);
        userContextMock.Setup(user => user.GetCurrentUser()).Returns(currentUser);

        CreateRestaurantCommandHandler createRestaurantCommandHandler = new(restaurantRepositoryMock.Object,
                                                                            loggerMock.Object,
                                                                            mapperMock.Object,
                                                                            userContextMock.Object);

        int result = await createRestaurantCommandHandler.Handle(command, CancellationToken.None);

        result.Should().Be(1);
        restaurant.OwnerId.Should().Be("owner-id");
        restaurantRepositoryMock.Verify(repository => repository.Create(restaurant), Times.Once());
    }
}